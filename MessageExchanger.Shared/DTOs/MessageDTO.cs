namespace MessageExchanger.Shared.DTOs
{
    public class MessageDTO
    {
        public Guid Id { get; set; }
        public string Contents { get; set; } = string.Empty;
        public string Signature { get; set; } = string.Empty;

        public string SenderUserName { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
    }
}
