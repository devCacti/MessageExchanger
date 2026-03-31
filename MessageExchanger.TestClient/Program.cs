using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using EI.SI;
using MessageExchanger.Shared.DTOs;
using MessageExchanger.Shared.Utils;

namespace MessageExchanger.TestClient
{
    class Program
    {
        static RSAParameters _privateKey;
        static RSAParameters _publicKey;
        static byte[] _symmetricKey = Array.Empty<byte>();

        static void Main()
        {
            Console.WriteLine("Connecting to server...");
            TcpClient client = new TcpClient("127.0.0.1", 9000);
            NetworkStream stream = client.GetStream();
            ProtocolSI protocol = new ProtocolSI();

            GenerateRSAKeys();

            // 1. Send PUBLIC_KEY
            byte[] publicKeyBytes = RSAKeyConverter.ToByteArray(_publicKey);
            byte[] packet = protocol.Make(ProtocolSICmdType.PUBLIC_KEY, publicKeyBytes);
            stream.Write(packet, 0, packet.Length);

            // 2. Receive SECRET_KEY
            stream.Read(protocol.Buffer, 0, protocol.Buffer.Length);
            if (protocol.GetCmdType() == ProtocolSICmdType.SECRET_KEY)
            {
                byte[] encryptedKey = protocol.GetData();
                _symmetricKey = DecryptSymmetricKey(encryptedKey);
                Console.WriteLine("Received symmetric key from server.");
            }

            // 3. Create LoginDTO
            var login = new LoginDTO
            {
                UserName = "admin",
                Password = "1234"
            };

            // 4. Encrypt LoginDTO with AES
            byte[] encryptedLogin = EncryptLoginDTO(login);

            // 5. Send SYM_CIPHER_DATA
            packet = protocol.Make(ProtocolSICmdType.SYM_CIPHER_DATA, encryptedLogin);
            stream.Write(packet, 0, packet.Length);

            // 6. Receive LOGIN_OK / LOGIN_FAIL
            stream.Read(protocol.Buffer, 0, protocol.Buffer.Length);
            Console.WriteLine("Server says: " + protocol.GetStringFromData());
        }

        static void GenerateRSAKeys()
        {
            using var rsa = RSA.Create(2048);
            _privateKey = rsa.ExportParameters(true);
            _publicKey = rsa.ExportParameters(false);
        }

        static byte[] DecryptSymmetricKey(byte[] encrypted)
        {
            using var rsa = RSA.Create();
            rsa.ImportParameters(_privateKey);
            return rsa.Decrypt(encrypted, RSAEncryptionPadding.Pkcs1);
        }

        static byte[] EncryptLoginDTO(LoginDTO dto)
        {
            byte[] json = JsonSerializer.SerializeToUtf8Bytes(dto);

            using var aes = Aes.Create();
            aes.Key = _symmetricKey;
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();
            byte[] cipher = encryptor.TransformFinalBlock(json, 0, json.Length);

            // Prepend IV
            return aes.IV.Concat(cipher).ToArray();
        }
    }

}
