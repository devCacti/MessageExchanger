namespace MessageExchanger.Shared.DTOs
{
    public class SymmetricKeyDTO
    {
        public byte[] EncryptedSymmetricKey { get; set; } = Array.Empty<byte>();
    }
}
