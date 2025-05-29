namespace Domain.DTOs
{
    public class ResetPasswordDTO
    {
        public required string Email { get; set; }
        public required string Pin { get; set; }
        public required string NewPassword { get; set; }
    }
}
