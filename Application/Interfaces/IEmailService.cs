using Application.DTOs.Email;

namespace Application.Interfaces
{
    public interface IEmailService
    {
        public Task SendAsync(SendEmailDto emailDto);
    }
}
