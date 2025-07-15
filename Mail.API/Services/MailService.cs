using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using Mail.API.Settings;
using MailKit.Security;

namespace Mail.API.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(string tomail, string subject, string body)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("BilalAPI", _mailSettings.From));
            emailMessage.To.Add(new MailboxAddress("Recipient", tomail));
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var smtpClient = new SmtpClient())
            {

                await smtpClient.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(_mailSettings.Username, _mailSettings.Password);
                await smtpClient.SendAsync(emailMessage);
                await smtpClient.DisconnectAsync(true);
            }
        }
    }
}
