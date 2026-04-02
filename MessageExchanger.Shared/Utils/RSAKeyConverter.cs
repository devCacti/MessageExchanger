using System.Security.Cryptography;
using System.Text.Json;

namespace MessageExchanger.Shared.Utils
{
    public class RsaPublicKey
    {
        public byte[] Modulus { get; set; } = Array.Empty<byte>();
        public byte[] Exponent { get; set; } = Array.Empty<byte>();
    }

    public static class RSAKeyConverter
    {
        public static byte[] ToByteArray(RSAParameters parameters)
        {
            var key = new RsaPublicKey
            {
                Modulus = parameters.Modulus ?? Array.Empty<byte>(),
                Exponent = parameters.Exponent ?? Array.Empty<byte>()
            };

            return JsonSerializer.SerializeToUtf8Bytes(key);
        }

        public static RSAParameters FromByteArray(byte[] data)
        {
            var key = JsonSerializer.Deserialize<RsaPublicKey>(data);

            if (key == null || key.Modulus == null || key.Exponent == null)
                throw new Exception("Invalid RSA public key received.");

            return new RSAParameters
            {
                Modulus = key.Modulus,
                Exponent = key.Exponent
            };
        }
    }
}
