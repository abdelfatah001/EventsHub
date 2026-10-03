using Domain.Entities;
using Domain.Interfaces;

namespace Application.Refresh_Token
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepo _tokenRepo;

        public RefreshTokenService (IRefreshTokenRepo tokenRepo)
        {
            _tokenRepo = tokenRepo;
        }

        public async Task<int> AddAsync(RefreshToken token)
        {
            if (token == null)
                throw new ArgumentNullException("Refresh token payload can't be null");

            return await _tokenRepo.AddAsync(token);
        }

        public async Task<RefreshToken?> GetActiveTokenByUserIdAsync(int Id)
        {
            if (Id <= 0) 
                throw new ArgumentOutOfRangeException("Refresh token id must be positive integer");

            return await _tokenRepo.GetActiveTokenByUserIdAsync(Id);
        }

        public async Task RevokeAsync(RefreshToken token)
        {
            if (token == null)
                throw new ArgumentNullException("Refresh token payload can't be null");

            await _tokenRepo.RevokeAsync(token);
        }
    }
    
}
