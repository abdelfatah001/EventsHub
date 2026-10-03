using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class VerificationCodeRepo : IVerificationCodeRepo
    {
        private readonly AppDbContext _context;

        public VerificationCodeRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<VertificationCode?> GetActiveByUserIdAsync(int userId, enVerificationPurpose purpose)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, nameof(userId));

            return await _context.VertificationCodes
                .FirstOrDefaultAsync(x => 
                x.UserId == userId && 
                x.UsedAt == null &&
                x.ExpiresAt > DateTime.UtcNow && 
                x.IsActive == true &&
                x.Purpose == purpose);
        }
        public async Task AddAsync(VertificationCode verificationCode)
        {
            ArgumentNullException.ThrowIfNull(verificationCode, nameof(verificationCode));

            _context.VertificationCodes.Add(verificationCode);

            await _context.SaveChangesAsync();

        }
        public async Task DeleteAsync(VertificationCode vertificationCode)
        {
            ArgumentNullException.ThrowIfNull(vertificationCode, nameof(vertificationCode));

            _context.VertificationCodes.Remove(vertificationCode);

            await _context.SaveChangesAsync();

        }

        public async Task UpdateAsync(VertificationCode verificationCode)
        {
            ArgumentNullException.ThrowIfNull(verificationCode, nameof(verificationCode));

            _context.VertificationCodes.Update(verificationCode);

            await _context.SaveChangesAsync();
        }
    }
}
