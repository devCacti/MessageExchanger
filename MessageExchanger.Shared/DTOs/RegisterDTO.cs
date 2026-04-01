namespace MessageExchanger.Shared.DTOs
{
    public class RegisterDTO
    {
        // Required Fields
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Optional Fields
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
