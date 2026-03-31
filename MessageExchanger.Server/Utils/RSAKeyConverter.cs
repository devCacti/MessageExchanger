using System.Security.Cryptography;
using System.Text.Json;

namespace MessageExchanger.Server.Utils
{
    public static class RSAKeyConverter
    {
        public static byte[] ToByteArray(RSAParameters parameters)
        {
            return JsonSerializer.SerializeToUtf8Bytes(parameters);
        }

        public static RSAParameters FromByteArray(byte[] data)
        {
            return JsonSerializer.Deserialize<RSAParameters>(data);
        }
    }
}
