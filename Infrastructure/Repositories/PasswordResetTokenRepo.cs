using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PasswordResetTokenRepo : IPasswordResetTokenRepo
    {
        private readonly AppDbContext _context;

        public PasswordResetTokenRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PasswordResetToken> AddAsync(PasswordResetToken resetToken)
        {
            await _context.PasswordResetTokens.AddAsync(resetToken);
            await _context.SaveChangesAsync();

            return resetToken;
        }


        public async Task<PasswordResetToken?> GetByIdAsync(int Id)
        {
            return await _context.PasswordResetTokens
                .FirstOrDefaultAsync(x => x.Id == Id );
        }


        public async Task<PasswordResetToken?> GetByUserIdAsync(int UserId)
        {
            return await _context.PasswordResetTokens
                .FirstOrDefaultAsync(
                x => x.UserId == UserId &&
                x.ExpiresAt > DateTime.UtcNow &&
                x.UsedAt == null);
        }

        public async Task<List<PasswordResetToken>> GetAllByUserIdAsync(int userId)
        {
            return await _context.PasswordResetTokens
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task UseItAsync(PasswordResetToken resetToken)
        {
            if (resetToken.UsedAt != null)
                throw new InvalidOperationException("Token has already been used.");

            resetToken.UsedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task InactivateAsync(PasswordResetToken resetToken)
        {
            if (!resetToken.IsActive)
                return;

            resetToken.IsActive = false;

            await _context.SaveChangesAsync();
        }
    }
}
