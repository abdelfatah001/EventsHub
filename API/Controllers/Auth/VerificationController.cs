using API.DTOs.Auth.Request;
using Application.Vertification;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers.Auth 
{

    [ApiController]
    [Route("api/[controller]")]
    public class VerificationController : ControllerBase
    {
        private readonly IVerificationService _verificationService;

        public VerificationController(IVerificationService verificationService)
        {
            _verificationService = verificationService;
        }

        [EnableRateLimiting("AuthLimiter")]
        [HttpPost("verifyCode")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyEmailDto dto, enVerificationPurpose purpose)
        {
            await _verificationService.VerifyAsync(dto.UserId, dto.Code, purpose);

            return Ok("Email verified successfully.");
        }

        [EnableRateLimiting("AuthLimiter")]
        [HttpPost("resendVerification")]
        public async Task<IActionResult> ResendVerification([FromBody] ResendVerificationDto dto, enVerificationPurpose purpose)
        {
            await _verificationService.ResendCodeAsync(dto.UserId, purpose);

            return Ok("A new verification code has been sent.");
        }
    }
}

