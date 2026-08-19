using Domain.Entities;
using Domain.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Email
{
    public class StmpEmailSender : IStmpEmailSender
    {
        private readonly EmailSettings _emailSettings;
        public StmpEmailSender(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public async Task<bool> SendAsync(string toEmail, string subject ,string TextBody, string content)
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);

                var bodyBuilder = new BodyBuilder
                {
                    TextBody = TextBody,
                    HtmlBody = content
                };

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Email));
                message.To.Add(MailboxAddress.Parse(toEmail));
                message.Subject = subject;
                message.Body = bodyBuilder.ToMessageBody();

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
           
            return true;
        }
    }
}
