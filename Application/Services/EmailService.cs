using Application.Helpers;
using Application.Interfaces.IServices;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;

using MimeKit;

namespace Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailtrapSetting _settings;

        public EmailService(IOptions<MailtrapSetting> options)
        {
            _settings = options.Value;
        }
        public async Task SendResetPinEmail(string toEmail, string pin)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("hello@demomailtrap.co"));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "Código de recuperación de cuenta";

            email.Body = new TextPart("plain")
            {
                Text = $"Tu código para restablecer tu contraseña es: {pin}"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("live.smtp.mailtrap.io", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
