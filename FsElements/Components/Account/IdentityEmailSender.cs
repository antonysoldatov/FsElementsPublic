using FsElements.Data;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using MimeKit.Text;

namespace FsElements.Components.Account
{
    public class IdentityEmailSender : IEmailSender<FsUser>
    {
        private readonly IConfiguration _configuration;

        public IdentityEmailSender(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public Task SendConfirmationLinkAsync(FsUser user, string email, string confirmationLink) =>
            SendEmailAsync(email, "Confirm your email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

        public Task SendPasswordResetCodeAsync(FsUser user, string email, string resetCode) => 
            SendEmailAsync(email, "Reset your password", $"Please reset your password using the following code: {resetCode}");

        public Task SendPasswordResetLinkAsync(FsUser user, string email, string resetLink) =>
            SendEmailAsync(email, "Reset your password", $"Please reset your password by <a href='{resetLink}'>clicking here</a>.");

        private async Task SendEmailAsync(string toEmail, string subject, string message)
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
