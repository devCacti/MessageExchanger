using System.ComponentModel.DataAnnotations;

namespace MessageExchanger.Server.Data.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // Credentials
        [Required]
        public string UserName { get; set; } = string.Empty;

        // This password system will use MD5 Hashing
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Salt { get; set; } = string.Empty;

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        // Relationships
        public ICollection<Message>? MessagesSent { get; set; }
    }
}
