using Application.ResetPasswordToken;
using Application.Users.Dto.Request;
using Domain.Entities;
using Domain.Exceptions;


namespace Application.Users
{
    public partial class UserService
    {

        public async Task PasswordResetRequest (int userId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId), userId, "User id must be a positive integer.");

            var user = await _userRepo.GetByIdAsync(userId);

            if (user is null)
                throw new KeyNotFoundException($"User with ID {userId} does not exist.");

            // generate a verification code and send it to the user's email for password reset
            // and save it as record in database

            await _verificationService.SendCodeAsync
                (user, Domain.Enums.enVerificationPurpose.PasswordReset);
        }

        public async Task ResetPassword (ResetPasswordDto dto)
        {
            var user = await _userRepo.GetByIdAsync(dto.UserId);

            if (user is null)
                throw new KeyNotFoundException($"User with ID {dto.UserId} does not exist.");


            bool isTokenValid = await _resetTokenService.ValidateTokenAsync(dto.UserId, dto.ResetToken);

            if (isTokenValid)
                throw new BusinessException("The provided reset token does not match the issued token or has expired.");

            // Update the user's password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            await _userRepo.UpdateAsync(user);
        }
    }
}
