using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace FsElements.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var host = this._configuration["SmtpSettings:Host"];
            var port = this._configuration.GetValue<int>("SmtpSettings:Port");
            var enableSsl = this._configuration.GetValue<bool>("SmtpSettings:EnableSsl");
            var userName = this._configuration["SmtpSettings:Username"];
            var passsword = this._configuration["SmtpSettings:Password"];


            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(userName));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = message };

            using var client = new SmtpClient();
            client.CheckCertificateRevocation = false;
            await client.ConnectAsync(host, port, enableSsl);
            await client.AuthenticateAsync(userName, passsword);
            await client.SendAsync(email);
            await client.DisconnectAsync(true);
        }
    }
}
