using System.Net;
using System.Net.Sockets;
using EI.SI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MessageExchanger.Server.Data;
using System.Security.Cryptography;
using MessageExchanger.Server.Services;
using MessageExchanger.Shared.Utils;

namespace MessageExchanger.Server
{
    public class Program
    {
        private static readonly List<TcpClient> ConnectedClients = new();
        private static AppDbContext _db = null!;
        private static Authenticator _auth = null!;

        private const int PORT = 9000;

        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            string connectionString = config.GetConnectionString("DefaultConnection") ?? throw new Exception("Connection string not found in configuration.");

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            _db = new AppDbContext(options);
            _auth = new Authenticator(_db);
            _db.Database.EnsureCreated();

            PrintBanner(PORT);

            TcpListener listener = new TcpListener(IPAddress.Any, PORT);
            listener.Start();

            Console.WriteLine($"[{DateTime.Now:T}] Listening on port {PORT}");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();

                lock (ConnectedClients)
                    ConnectedClients.Add(client);

                Console.WriteLine($"[{DateTime.Now:T}] Client connected");

                Thread t = new Thread(() => HandleClient(client));
                t.Start();
            }
        }

        private static void HandlePublicKey(TcpClient client, ProtocolSI protocol, NetworkStream stream)
        {
            // 1. Extract client public key
            byte[] keyBytes = protocol.GetData();

            Console.WriteLine($"Received PUBLIC_KEY bytes: {keyBytes.Length}");
            Console.WriteLine(BitConverter.ToString(keyBytes));

            RSAParameters clientKey = RSAKeyConverter.FromByteArray(keyBytes);

            _auth.StoreClientPublicKey(client, clientKey);

            // 2. Generate symmetric key
            _auth.GenerateSymmetricKeyForClient(client);

            // 3. Encrypt symmetric key with client public key
            byte[] encryptedKey = _auth.EncryptSymmetricKeyForClient(client);

            // 4. Send SECRET_KEY packet
            var response = new ProtocolSI();
            byte[] packet = response.Make(ProtocolSICmdType.SECRET_KEY, encryptedKey);
            stream.Write(packet, 0, packet.Length);

            Console.WriteLine($"[{DateTime.Now:T}] Sent encrypted symmetric key to client");
        }

        private static void HandleEncryptedLogin(TcpClient client, ProtocolSI protocol, NetworkStream stream)
        {
            byte[] encryptedPayload = protocol.GetData();

            var loginDto = _auth.DecryptLoginDTO(client, encryptedPayload);

            bool valid = _auth.ValidateCredentials(loginDto);

            var response = new ProtocolSI();
            byte[] packet = response.Make(
                ProtocolSICmdType.DATA,
                valid ? "LOGIN_OK" : "LOGIN_FAIL"
            );

            stream.Write(packet, 0, packet.Length);

            if (valid)
            {
                _auth.MarkAuthenticated(client, loginDto.UserName);
                Console.WriteLine($"[{DateTime.Now:T}] User '{loginDto.UserName}' authenticated");
            }
            else
            {
                Console.WriteLine($"[{DateTime.Now:T}] Failed login attempt for '{loginDto.UserName}'");
            }
        }



        private static void HandleClient(TcpClient client)
        {
            var protocol = new ProtocolSI();
            NetworkStream stream = client.GetStream();
            _auth.RegisterClient(client);

            try
            {
                while (client.Connected)
                {
                    int bytes = stream.Read(protocol.Buffer, 0, protocol.Buffer.Length);
                    if (bytes == 0) break;

                    var cmd = protocol.GetCmdType();

                    switch (cmd)
                    {
                        case ProtocolSICmdType.PUBLIC_KEY:
                            HandlePublicKey(client, protocol, stream);
                            break;

                        case ProtocolSICmdType.SYM_CIPHER_DATA:
                            HandleEncryptedLogin(client, protocol, stream);
                            break;

                        case ProtocolSICmdType.DATA:
                            if (!_auth.IsAuthenticated(client))
                            {
                                Console.WriteLine("Unauthenticated client attempted to send DATA");
                                break;
                            }

                            string msg = protocol.GetStringFromData();
                            Console.WriteLine($"[{DateTime.Now:T}] {msg}");
                            Broadcast(msg, client);
                            break;

                        case ProtocolSICmdType.EOT:
                            Console.WriteLine($"[{DateTime.Now:T}] Client disconnected");
                            client.Close();
                            return;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now:T}] Client error: {ex.Message}");
            }
        }

        private static void Broadcast(string message, TcpClient sender)
        {
            var protocol = new ProtocolSI();
            byte[] packet = protocol.Make(ProtocolSICmdType.DATA, message);

            lock (ConnectedClients)
            {
                foreach (var client in ConnectedClients)
                {
                    if (client != sender && client.Connected)
                    {
                        client.GetStream().Write(packet, 0, packet.Length);
                    }
                }
            }
        }

        private static void PrintBanner(int port)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("|=========================================|");
            Console.WriteLine("|         MessageExchanger Server         |");
            Console.WriteLine("|=========================================|");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Running on port: {port}");
            Console.ResetColor();

            Console.WriteLine();
        }
    }
}
