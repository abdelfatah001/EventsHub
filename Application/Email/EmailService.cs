using Application.DTOs.Email;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Email
{
    public class EmailService : IEmailService
    {
        private readonly IStmpEmailSender _emailSender;

        public EmailService(IStmpEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task SendAsync(SendEmailDto emailDto)
        {
            if (emailDto == null)
            {
                throw new ArgumentNullException(nameof(emailDto), "Email data cannot be null.");
            }

            await _emailSender.SendAsync(emailDto.To, emailDto.Subject, emailDto.TextBody, emailDto.HtmlBody);
        }
    }
}
