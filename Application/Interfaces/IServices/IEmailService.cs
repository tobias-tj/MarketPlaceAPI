namespace Application.Interfaces.IServices
{
    public interface IEmailService
    {
        public Task SendResetPinEmail(string toEmail, string pin);
    }
}
