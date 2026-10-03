using Application.Email;
using Application.Email.Dto;
using Application.ResetPasswordToken;
using Application.Users;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Vertification
{
    public class VerificationService : IVerificationService
    {
        private readonly IUserRepo _userRepo;
        private readonly IVerificationCodeRepo _verificationRepo;
        private readonly IPasswordHasher<VertificationCode> _passwordHasher;
        private readonly IEmailService _emailService;
        private readonly IPasswordResetTokenService _resetTokenservice;
        private readonly VerificationEmailBuilder _emailBuilder;


        public VerificationService (IVerificationCodeRepo vertificationRepo,
            IPasswordHasher<VertificationCode> passwordHasher, IEmailService emailService, 
            VerificationEmailBuilder emailBuilder)
        {
            _verificationRepo = vertificationRepo;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
            _emailBuilder = emailBuilder;
        }

        private string GenerateVerificationCode()
        {
            return RandomNumberGenerator
                .GetInt32(100000, 1000000)
                .ToString();
        }

        public async Task SendCodeAsync(User user, enVerificationPurpose verificationPurpose)
        {

            if (user == null)
                throw new KeyNotFoundException($"User with Id {user.Id} is not found.");


            var verCode = await _verificationRepo.GetActiveByUserIdAsync(user.Id, verificationPurpose);

            // if there a previous code inactivate it
            if (verCode != null)
            {
                verCode.IsActive = false;
                await _verificationRepo.UpdateAsync(verCode);
            }

            string verCodeString = GenerateVerificationCode();

            var newVerCode = new VertificationCode
            {
                UserId = user.Id,
                Email = user.Email,
                Purpose = verificationPurpose,
                ExpiresAt = DateTime.UtcNow.AddMinutes(2),
                CreatedAt = DateTime.UtcNow,
                UsedAt = null,
                IsActive = true,
                Attempts = 0
            };

            newVerCode.CodeHash = _passwordHasher.HashPassword(newVerCode, verCodeString);

            await _verificationRepo.AddAsync(newVerCode);

            var vertificationEmail = _emailBuilder.Build(newVerCode.Email, verCodeString, verificationPurpose);

            await _emailService.SendAsync(vertificationEmail);

        }

        public async Task<bool> VerifyAsync(int userId, string code, enVerificationPurpose purpose)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId), userId, "User id must be a positive integer.");

            ArgumentException.ThrowIfNullOrWhiteSpace(code, "Code is required");

            var user = await _userRepo.GetByIdAsync(userId);

            if (user == null)
                throw new KeyNotFoundException($"User with Id {userId} is not found.");


            var verCode = await _verificationRepo.GetActiveByUserIdAsync(userId, purpose);

            if (verCode == null)
                throw new BusinessException("Vertification Code hasn't sent yet.");

            if (verCode.ExpiresAt > DateTime.UtcNow)
            {
                verCode.IsActive = false;
                await _verificationRepo.UpdateAsync(verCode);

                throw new BusinessException("The verification code you sent is expired");
            }

            if (verCode.Attempts >= 3)
                throw new BusinessException("Maximum verification attempts exceeded.");

            var result = _passwordHasher.VerifyHashedPassword(verCode, verCode.CodeHash, code);


            if (result == PasswordVerificationResult.Success)
            {
                await CodeVerificationSucceed(verCode, user, purpose);

                return true;
            }

            else
            {
                verCode.Attempts++;
                await _verificationRepo.UpdateAsync(verCode);

                throw new Exception("Your code is wrong");
            }
           

        }

        public async Task ResendCodeAsync (int userId, enVerificationPurpose verificationPurpose)
        {
            var user = await _userRepo.GetByIdAsync(userId);

            if (user == null)
                throw new KeyNotFoundException($"User with Id {userId} is not found.");

            await SendCodeAsync(user, verificationPurpose);
        }

        private async Task CodeVerificationSucceed (VertificationCode verCode, User user, enVerificationPurpose purpose)
        {
            verCode.UsedAt = DateTime.UtcNow;
           
            if (purpose == enVerificationPurpose.EmailVerification)
                user.IsEmailVerified = true;

            if (purpose == enVerificationPurpose.PasswordReset)
                await _resetTokenservice.CreateResetTokenAsync(user.Id);


            await _verificationRepo.UpdateAsync(verCode);

        }


    }
}
