using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IRefreshTokenRepo
    {
        Task<int> AddAsync(RefreshToken token);
        Task RevokeAsync(RefreshToken token);
        Task<RefreshToken?> GetActiveTokenByUserIdAsync(int Id);

    }
}
