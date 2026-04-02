using System.Net;
using System.Net.Sockets;
using EI.SI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MessageExchanger.Server.Data;
using System.Security.Cryptography;
using MessageExchanger.Server.Services;
using MessageExchanger.Server.Data.Entities;
using MessageExchanger.Shared.Utils;
using MessageExchanger.Shared.DTOs;
using System.Text.Json;

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

            // Pull the key from the new section
            // This master key should not be in *appsettings.json* in a production environment, but for the sake of this demo it will be saved here.
            string masterKeyBase64 = config["SecuritySettings:DbMasterKey"]
                ?? throw new Exception("Master Key missing from config!");

            // Pass it to your service (you can make the service take the key in a 'Initialize' method)
            EncryptionService.Initialize(masterKeyBase64);

            string connectionString = config.GetConnectionString("DefaultConnection") ?? throw new Exception("Connection string not found in configuration.");

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            _db = new AppDbContext(options);
            _auth = new Authenticator(_db);
            _db.Database.Migrate();

            PrintBanner(PORT);

            TcpListener listener = new TcpListener(IPAddress.Any, PORT);
            listener.Start();

            Logger.Log($"Server started on port {PORT}. Waiting for clients...");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();

                lock (ConnectedClients)
                    ConnectedClients.Add(client);

                Logger.Log("Client Connected");

                Thread t = new Thread(() => HandleClient(client));
                t.Start();
            }
        }

        private static void HandlePublicKey(TcpClient client, ProtocolSI protocol, NetworkStream stream)
        {
            // 1. Extract client public key
            byte[] keyBytes = protocol.GetData();

            Logger.Log($"Received public key from client");

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

            Logger.Log($"Sent encrypted symmetric key to client");
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
                Logger.Log($"User '{loginDto.UserName}' authenticated successfully");
            }
            else
            {
                Logger.Log($"Failed login attempt for '{loginDto.UserName}'");
            }
        }

        private static void HandleEncryptedRegister(TcpClient client, ProtocolSI protocol, NetworkStream stream)
        {
            byte[] encryptedPayload = protocol.GetData();

            var registerDto = _auth.DecryptRegisterDTO(client, encryptedPayload);
            
            bool success = _auth.RegisterUser(registerDto);

            var response = new ProtocolSI();
            byte[] packet = response.Make(
                ProtocolSICmdType.DATA,
                success ? "REGISTER_OK" : "REGISTER_FAIL"
            );

            stream.Write(packet, 0, packet.Length);

            if (success)
            {
                Logger.Log($"User '{registerDto.UserName}' registered successfully");
            }
            else
            {
                Logger.Log($"Failed registration attempt for '{registerDto.UserName}'");
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
                    // Clears the buffer before each read to prevent leftover data from previous messages causing issues.
                    Array.Clear(protocol.Buffer, 0, protocol.Buffer.Length);

                    int bytes = stream.Read(protocol.Buffer, 0, protocol.Buffer.Length);
                    if (bytes == 0) break;

                    var cmd = protocol.GetCmdType();

                    switch (cmd)
                    {
                        case ProtocolSICmdType.PUBLIC_KEY:
                            Logger.Log($"Received PUBLIC_KEY command from client. Processing key exchange...");
                            HandlePublicKey(client, protocol, stream);
                            break;

                        // Login Section - expects encrypted LoginDTO
                        case ProtocolSICmdType.SYM_CIPHER_DATA:
                            if (!_auth.IsAuthenticated(client))
                            {
                                Logger.Log($"Received encrypted data from unauthenticated client. Attempting to process as login...");
                                HandleEncryptedLogin(client, protocol, stream);
                            }
                            else
                            {
                                Logger.Log($"Received encrypted message from authenticated client. Attempting to process as direct message...");
                                HandleDirectMessage(client, protocol);
                            }

                            break;

                        // Register Section - Will be implemented to allow for correct user registration in the database.
                        case ProtocolSICmdType.USER_OPTION_1:
                            Logger.Log($"Received registration request from client");
                            HandleEncryptedRegister(client, protocol, stream);
                            break;

                        case ProtocolSICmdType.EOT:
                            Logger.Log($"Client disconnected");
                            client.Close();
                            return;
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Client error: {ex.Message}");
            }
            finally
            {
                _auth.UnregisterClient(client);
                lock (ConnectedClients) ConnectedClients.Remove(client);
                client.Close();
            }
        }

        private static void HandleDirectMessage(TcpClient senderClient, ProtocolSI protocol)
        {
            try
            {
                // 1. Read as a Base64 string instead of raw bytes to avoid buffer corruption
                //string base64Payload = protocol.GetStringFromData().Replace("\0", "");
                byte[] rawBuffer = protocol.GetData();

                byte[] encryptedPayload = rawBuffer.Take(protocol.GetDataLength()).ToArray();
                byte[] decryptedPayload = _auth.DecryptDataFromClient(senderClient, encryptedPayload);
                var createDto = JsonSerializer.Deserialize<MessageCreateDTO>(decryptedPayload);

                if (createDto == null) return;

                string senderUsername = _auth.GetAuthenticatedUsername(senderClient);

                bool isSignatureValid = _auth.VerifySignature(senderClient, createDto.Contents, createDto.Signature);

                if (!isSignatureValid)
                {
                    Logger.Log($"WARNING: Invalid signature from {senderUsername}. Message dropped.");
                    return;
                }

                // --- 3. PERSISTENCE & ROUTING (Existing) ---
                Logger.Log($"Signature verified for {senderUsername}. Processing...");

                User senderUser = _db.Users.FirstOrDefault(u => u.UserName == senderUsername) ?? throw new Exception("Authenticated user not found in database");
                User receiverUser = _db.Users.FirstOrDefault(u => u.UserName == createDto.ReceiverUserName) ?? throw new Exception("Receiver user not found in database");

                Message msg = new Message
                {
                    Contents = EncryptionService.EncryptForDb(createDto.Contents), // Encrypts with master key
                    Signature = createDto.Signature,
                    Sender = senderUser,
                    Receiver = receiverUser,
                    SentAt = DateTime.UtcNow
                };

                // Save message on the database (No encryption for now)
                Logger.Log($"Saving message from {senderUsername} to {createDto.ReceiverUserName} in database");
                _db.Messages.Add(msg);
                _db.SaveChanges();

                // NOTE: Ensure your DTO property name matches here! (ReceiverUsername)
                Logger.Log($"Routing message from {senderUsername} to {createDto.ReceiverUserName}");

                var outDto = new MessageDTO
                {
                    SenderUserName = senderUsername,
                    Contents = createDto.Contents,
                    Signature = createDto.Signature,
                    SentAt = DateTime.UtcNow
                };

                TcpClient? recipientClient = _auth.GetClientByUsername(createDto.ReceiverUserName);

                if (recipientClient != null && recipientClient.Connected)
                {
                    byte[] payloadForRecipient = _auth.EncryptDataForClient(recipientClient, JsonSerializer.SerializeToUtf8Bytes(outDto));

                    var responseProtocol = new ProtocolSI();
                    byte[] packet = responseProtocol.Make(ProtocolSICmdType.SYM_CIPHER_DATA, payloadForRecipient);

                    recipientClient.GetStream().Write(packet, 0, packet.Length);
                    Logger.Log($"Message delivered to {createDto.ReceiverUserName}");
                }
                else
                {
                    Logger.Log($"User {createDto.ReceiverUserName} is not currently connected. Will not receive this message.");
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Error handling direct message: {ex.Message}");
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
                    // Send to everyone EXCEPT the person who sent it
                    if (client != sender && client.Connected)
                    {
                        try
                        {
                            client.GetStream().Write(packet, 0, packet.Length);
                        }
                        catch
                        {
                            // Ignore clients that disconnected ungracefully
                        }
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
