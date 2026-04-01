using System.Security.Cryptography;
using MessageExchanger.Server.Data;
using MessageExchanger.Server.Data.Entities;
using MessageExchanger.Shared.DTOs;
using System.Text.Json;
using System.Net.Sockets;
using MessageExchanger.Server.Utils;

namespace MessageExchanger.Server.Services
{
    public class Authenticator
    {
        private readonly AppDbContext _db;

        public Authenticator(AppDbContext db)
        {
            _db = db;
            GenerateServerKeys();
        }

        // Server RSA keys
        private RSAParameters _serverPublicKey;
        private RSAParameters _serverPrivateKey;

        // Per-client session storage
        private readonly Dictionary<TcpClient, ClientSession> _sessions = new();

        public void GenerateServerKeys()
        {
            using var rsa = RSA.Create(2048);
            _serverPublicKey = rsa.ExportParameters(false);
            _serverPrivateKey = rsa.ExportParameters(true);
        }

        public void RegisterClient(TcpClient client)
        {
            _sessions[client] = new ClientSession();
        }

        public void StoreClientPublicKey(TcpClient client, RSAParameters publicKey)
        {
            _sessions[client].ClientPublicKey = publicKey;
        }

        public byte[] GenerateSymmetricKeyForClient(TcpClient client)
        {
            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.GenerateKey();

            _sessions[client].SymmetricKey = aes.Key;
            return aes.Key;
        }

        public byte[] EncryptSymmetricKeyForClient(TcpClient client)
        {
            var session = _sessions[client];

            using var rsa = RSA.Create();
            rsa.ImportParameters(session.ClientPublicKey);

            return rsa.Encrypt(session.SymmetricKey, RSAEncryptionPadding.Pkcs1);
        }

        public LoginDTO DecryptLoginDTO(TcpClient client, byte[] encryptedData)
        {
            var key = _sessions[client].SymmetricKey;

            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = encryptedData.Take(16).ToArray();

            using var decryptor = aes.CreateDecryptor();
            var cipher = encryptedData.Skip(16).ToArray();
            var plainBytes = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);

            return JsonSerializer.Deserialize<LoginDTO>(plainBytes)!;
        }

        public RegisterDTO DecryptRegisterDTO(TcpClient client, byte[] encryptedData)
        {
            var key = _sessions[client].SymmetricKey;

            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = encryptedData.Take(16).ToArray();

            using var decryptor = aes.CreateDecryptor();
            var cipher = encryptedData.Skip(16).ToArray();
            var plainBytes = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);

            return JsonSerializer.Deserialize<RegisterDTO>(plainBytes)!;
        }

        public bool RegisterUser(RegisterDTO dto)
        {
            // 1. Check if username already exists
            if (_db.Users.Any(u => u.UserName == dto.UserName))
                return false;

            // 2. Generate random salt
            string salt = SecurityUtils.GenerateSalt(); // or implement below

            // 3. Hash password + salt with MD5
            string hash = SecurityUtils.Md5Hash(dto.Password + salt); // or implement below

            // 4. Create user entity
            var user = new User
            {
                UserName = dto.UserName,
                Password = hash,
                Salt = salt,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            // 5. Save to DB
            _db.Users.Add(user);
            _db.SaveChanges();

            return true;
        }


        public bool ValidateCredentials(LoginDTO dto)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName == dto.UserName);
            if (user == null) return false;

            string computedHash = SecurityUtils.Md5Hash(dto.Password + user.Salt);
            return computedHash == user.Password;
        }


        public void MarkAuthenticated(TcpClient client, string username)
        {
            _sessions[client].Authenticated = true;
            _sessions[client].Username = username;
        }

        public bool IsAuthenticated(TcpClient client)
        {
            return _sessions.ContainsKey(client) && _sessions[client].Authenticated;
        }
    }

    public class ClientSession
    {
        public RSAParameters ClientPublicKey { get; set; }
        public byte[] SymmetricKey { get; set; } = Array.Empty<byte>();
        public bool Authenticated { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}
