using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using System.Security.Cryptography;


namespace Application.ResetPasswordToken
{
    public class PasswordResetTokenService: IPasswordResetTokenService
    {
        private readonly IPasswordResetTokenRepo _passwordResetRepo;

        public PasswordResetTokenService( IPasswordResetTokenRepo passwordResetRepo)
        {
            _passwordResetRepo = passwordResetRepo;
        }

        private string GenerateToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);

            return Convert.ToBase64String(bytes);
        }

        public async Task<string> CreateResetTokenAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException(
                    "UserId must be greater than zero.",
                    nameof(userId));

            var tokens = await _passwordResetRepo.GetAllByUserIdAsync(userId);

            // Inactivate previous active tokens
            foreach (var token in tokens.Where(x => x.IsActive))
            {
                await _passwordResetRepo.InactivateAsync(token);
            }

            // Generate raw token
            var rawToken = GenerateToken();

            // Hash token before storing it
            var tokenHash = BCrypt.Net.BCrypt.HashPassword(rawToken);

            var resetToken = new PasswordResetToken
            {
                UserId = userId,
                TokenHash = tokenHash,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                IsActive = true,
                UsedAt = null
            };

            // Save and get the created entity
            var createdToken =
                await _passwordResetRepo.AddAsync(resetToken);

            return rawToken;
        }

        public async Task<PasswordResetToken> GetActiveByUserIdAsync(int UserId)
        {
            if (UserId <= 0)
                throw new ArgumentException("Id must be greater than zero.",nameof(UserId));

            var token = await _passwordResetRepo.GetByUserIdAsync(UserId);

            if (token == null)
                throw new KeyNotFoundException("Password reset token was not found.");

            return token;
        }

        public async Task<List<PasswordResetToken>> GetAllByUserIdAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be greater than zero.", nameof(userId));

            return await _passwordResetRepo.GetAllByUserIdAsync(userId);
        }

        public async Task InactivateAsync(int Id)
        {
            if (Id <= 0)
                throw new ArgumentException("Id must be greater than zero.", nameof(Id));

            var token = await _passwordResetRepo.GetByUserIdAsync(Id);

            if (token == null)
                throw new KeyNotFoundException(
                    "Password reset token was not found.");

            await _passwordResetRepo.InactivateAsync(token);
        }

        public async Task<bool> ValidateTokenAsync(int userId, string token)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be greater than zero.",nameof(userId));

            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be empty.", nameof(token));

            var activeToken = await _passwordResetRepo.GetByUserIdAsync(userId);

            if (activeToken == null)
                throw new KeyNotFoundException($"There is no active token for user with Id {userId}");

            if (activeToken.ExpiresAt < DateTime.UtcNow)
                throw new BusinessException("The token has expired.");

            return BCrypt.Net.BCrypt.Verify(token,activeToken.TokenHash);
            
        }

        public async Task<PasswordResetToken?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id must be greater than zero.", nameof(id));

            return await _passwordResetRepo.GetByIdAsync(id);
        }
    }
}
