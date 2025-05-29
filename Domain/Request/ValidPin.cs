namespace Domain.Request
{
    public class ValidPin
    {
        public required string Email { get; set; }
        public required string Pin { get; set; }
    }
}
