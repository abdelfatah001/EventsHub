using API.DTOs.Auth.Request;
using Application.Vertification;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
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

        [HttpPost("verifyCode")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyEmailDto dto, enVerificationPurpose purpose)
        {
            await _verificationService.VerifyAsync(dto.UserId, dto.Code, purpose);

            return Ok("Email verified successfully.");
        }

        [HttpPost("resendVerification")]
        public async Task<IActionResult> ResendVerification([FromBody] ResendVerificationDto dto, enVerificationPurpose purpose)
        {
            await _verificationService.ResendCodeAsync(dto.UserId, purpose);

            return Ok("A new verification code has been sent.");
        }
    }
}

