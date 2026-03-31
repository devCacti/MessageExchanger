namespace MessageExchanger.Shared.DTOs
{
    public class LoginResponseDTO
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public UserDTO? User { get; set; }
    }
}
