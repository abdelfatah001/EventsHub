using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPasswordResetTokenRepo
    {
        Task<PasswordResetToken> AddAsync(PasswordResetToken resetToken);
        Task<PasswordResetToken?> GetByUserIdAsync(int UserId);
        Task<List<PasswordResetToken>> GetAllByUserIdAsync(int userId);
        Task<PasswordResetToken?> GetByIdAsync(int Id);
        Task InactivateAsync(PasswordResetToken resetToken);

        Task UseItAsync(PasswordResetToken resetToken);
    }

}
