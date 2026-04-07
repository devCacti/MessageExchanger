using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessageExchanger.Server.Data.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // Credentials
        [Required]
        public string UserName { get; set; } = string.Empty;

        // This password system will use ~MD5 Hashing~ -> BCrypt
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Salt { get; set; } = string.Empty;

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        // Relationships
        [InverseProperty("Sender")] // Para o EFCore saber qual é a correspondencia
        public virtual ICollection<Message>? MessagesSent { get; set; }
        [InverseProperty("Receiver")]
        public virtual ICollection<Message>? MessagesReceived { get; set; }
    }
}
