using System.ComponentModel.DataAnnotations;

namespace MessageExchanger.Server.Data.Entities
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Contents { get; set; } = string.Empty;
        public string Signature { get; set; } = string.Empty;

        public User? Sender { get; set; }

        public DateTime SentAt { get; set; }
    }
}
