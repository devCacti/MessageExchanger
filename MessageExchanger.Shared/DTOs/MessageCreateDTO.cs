namespace MessageExchanger.Shared.DTOs
{
    public class MessageCreateDTO
    {
        public string ReceiverUserName { get; set; } = string.Empty;

        public string Contents { get; set; } = string.Empty;
        public string Signature { get; set; } = string.Empty;
    }
}
