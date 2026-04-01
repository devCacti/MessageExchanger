using System.Security.Cryptography;
using System.Text;

namespace MessageExchanger.Server.Utils
{
    public static class SecurityUtils
    {
        public static string GenerateSalt(int size = 16)
        {
            byte[] bytes = RandomNumberGenerator.GetBytes(size);
            return Convert.ToBase64String(bytes);
        }

        public static string Md5Hash(string input)
        {
            using var md5 = MD5.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] hash = md5.ComputeHash(bytes);
            return Convert.ToHexString(hash);
        }
    }

}
