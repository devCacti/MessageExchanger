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
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1 - Login");
            Console.WriteLine("2 - Register");
            Console.Write("Option: ");
            string option = Console.ReadLine()!;
            bool isLogin = option == "1";

            Console.Write("Username: ");
            string username = Console.ReadLine()!;

            Console.Write("Password: ");
            string password = ReadHiddenPassword();

            string? firstName = null;
            string? lastName = null;

            if (!isLogin)
            {
                Console.Write("First Name (optional): ");
                firstName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(firstName)) firstName = null;

                Console.Write("Last Name (optional): ");
                lastName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(lastName)) lastName = null;
            }

            Console.WriteLine("Connecting to server...");
            TcpClient client = new TcpClient("127.0.0.1", 9000);
            NetworkStream stream = client.GetStream();
            ProtocolSI protocol = new ProtocolSI();

            // ============================
            //   KEY EXCHANGE (ALWAYS)
            // ============================
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
            else
            {
                Console.WriteLine("Did not receive SECRET_KEY from server.");
                return;
            }

            // ============================
            //   BRANCH: LOGIN / REGISTER
            // ============================

            if (isLogin)
            {
                var login = new LoginDTO
                {
                    UserName = username,
                    Password = password
                };

                byte[] encryptedLogin = EncryptWithAes(JsonSerializer.SerializeToUtf8Bytes(login));
                packet = protocol.Make(ProtocolSICmdType.SYM_CIPHER_DATA, encryptedLogin);
                stream.Write(packet, 0, packet.Length);
            }
            else
            {
                var register = new RegisterDTO
                {
                    UserName = username,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName
                };

                byte[] encryptedRegister = EncryptWithAes(JsonSerializer.SerializeToUtf8Bytes(register));
                packet = protocol.Make(ProtocolSICmdType.USER_OPTION_1, encryptedRegister);
                stream.Write(packet, 0, packet.Length);
            }

            // 6. Receive server response
            stream.Read(protocol.Buffer, 0, protocol.Buffer.Length);
            Console.WriteLine("Server says: " + protocol.GetStringFromData());

            var eotPacket = protocol.Make(ProtocolSICmdType.EOT);
            stream.Write(eotPacket, 0, eotPacket.Length);
            client.Close();
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

        static byte[] EncryptWithAes(byte[] plain)
        {
            using var aes = Aes.Create();
            aes.Key = _symmetricKey;
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();
            byte[] cipher = encryptor.TransformFinalBlock(plain, 0, plain.Length);

            return aes.IV.Concat(cipher).ToArray();
        }


        static string ReadHiddenPassword()
        {
            StringBuilder sb = new StringBuilder();
            ConsoleKeyInfo key;

            while (true)
            {
                key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (sb.Length > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    sb.Append(key.KeyChar);
                    Console.Write("*");
                }
            }

            return sb.ToString();
        }

    }
}
