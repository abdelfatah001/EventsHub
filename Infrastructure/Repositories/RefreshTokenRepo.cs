using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RefreshTokenRepo : IRefreshTokenRepo
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepo (AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(RefreshToken token)
        {
            ArgumentNullException.ThrowIfNull(token);

            _context.RefreshTokens.Add(token);

            return await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?>  GetActiveTokenByUserIdAsync(int UserId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(UserId);

            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync
                    (t => t.UserId == UserId && 
                    t.RevokedAt == null && 
                    t.ExpiresAt > DateTime.UtcNow);

            return token;
        }

        public async Task RevokeAsync(RefreshToken token)
        {
            if (token == null) 
                throw new ArgumentNullException(nameof(token));

            token.RevokedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

        }

    }
}
