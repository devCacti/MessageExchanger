namespace MessageExchanger.Shared.DTOs
{
    public class EncryptedPacketDTO
    {
        public byte[] Payload { get; set; } = Array.Empty<byte>();
    }
}
