using System.Text;
using System.Security.Cryptography;

namespace MessageExchanger.Server.Services
{
    public static class EncryptionService
    {
        private static byte[] _masterKey = null!;

        public static void Initialize(string base64Key)
        {
            _masterKey = Convert.FromBase64String(base64Key);
        }

        public static string EncryptForDb(string plaintext)
        {
            if (_masterKey == null) throw new Exception("EncryptionService not initialized!");

            using var aes = Aes.Create();
            aes.Key = _masterKey;
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();
            byte[] plainBytes = Encoding.UTF8.GetBytes(plaintext);
            byte[] cipherText = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            // Result: [IV (16 bytes)][Ciphertext] converted to one Base64 string
            return Convert.ToBase64String(aes.IV.Concat(cipherText).ToArray());
        }

        public static string DecryptFromDb(string encryptedData)
        {
            if (_masterKey == null) throw new Exception("EncryptionService not initialized!");

            byte[] combinedData = Convert.FromBase64String(encryptedData);
            byte[] iv = combinedData.Take(16).ToArray();
            byte[] cipherText = combinedData.Skip(16).ToArray();

            using var aes = Aes.Create();
            aes.Key = _masterKey;
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            byte[] plainBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
