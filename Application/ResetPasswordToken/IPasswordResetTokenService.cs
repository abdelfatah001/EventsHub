using Domain.Entities;

namespace Application.ResetPasswordToken
{
    public interface IPasswordResetTokenService
    {
        Task<string> CreateResetTokenAsync(int userId);

        Task<PasswordResetToken?> GetByIdAsync(int id);

        Task<List<PasswordResetToken>> GetAllByUserIdAsync(int userId);

        Task InactivateAsync(int id);

        Task<bool> ValidateTokenAsync(int userId, string token);
    }
}
