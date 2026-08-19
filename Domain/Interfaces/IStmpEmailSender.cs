
namespace Domain.Interfaces
{
    public interface IStmpEmailSender
    {
        Task<bool> SendAsync(string toEmail, string subject, string TextBody, string content);
    }
}
