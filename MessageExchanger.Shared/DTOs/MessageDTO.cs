namespace MessageExchanger.Shared.DTOs
{
    public class MessageDTO
    {
        public Guid Id { get; set; }
        public string Contents { get; set; } = string.Empty;
        public string Signature { get; set; } = string.Empty;

        public Guid SenderId { get; set; }
        public DateTime SentAt { get; set; }
    }
}
