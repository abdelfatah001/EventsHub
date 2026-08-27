using Application.Email.Dto;

namespace Application.Email
{
    public interface IEmailService
    {
        public Task SendAsync(SendEmailDto emailDto);
    }
}
