using Domain.Entities;

namespace Application.Refresh_Token
{
    public interface IRefreshTokenService
    {
        Task<int> AddAsync(RefreshToken token);
        Task RevokeAsync(RefreshToken token);
        Task<RefreshToken?> GetActiveTokenByUserIdAsync(int Id);

    }
}
