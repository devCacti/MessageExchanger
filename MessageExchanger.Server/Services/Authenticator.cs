using System.Security.Cryptography;
using System.Text;
using MessageExchanger.Server.Data;
using MessageExchanger.Shared.DTOs;
using System.Text.Json;
using System.Net.Sockets;

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

        public bool ValidateCredentials(LoginDTO dto)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName == dto.UserName);

            if (user == null)
                return false;

            return user.Password == dto.Password; // Phase 1 (plaintext)
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
        public byte[] SymmetricKey { get; set; }
        public bool Authenticated { get; set; }
        public string Username { get; set; }
    }
}
