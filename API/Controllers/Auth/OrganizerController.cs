using API.Controllers;
using Application.Organizers;
using Application.ResetPasswordToken;
using Application.Users;
using Application.Users.Dto.Request;
using Application.Vertification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers.Auth
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizerController : BaseController
    {
        private readonly IOrganizerService _organizerService;
        private readonly IVerificationService _verificationService;
        private readonly IPasswordResetTokenService _passwordResetService;

        public OrganizerController (IOrganizerService organizerService, IVerificationService verificationService, IPasswordResetTokenService passwordResetService)
        {
            _organizerService = organizerService;
            _verificationService = verificationService;
            _passwordResetService = passwordResetService;
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _organizerService.GetAllAsync();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetOrganizerById(int id)
        {
            var result = await _organizerService.GetByIdAsync(id);

            return result is not null ?
                Ok(result) :
                NotFound(new { message = $"Organizer with ID {id} not found." });
        }

        [Authorize]
        [HttpGet("GetProfileById/{id}")]
        public async Task<IActionResult> GetOrganizerProfileById(int id)
        {
            var result = await _organizerService.GetProfileByIdAsync(id);

            return result is not null ?
                Ok(result) :
                NotFound(new { message = $"Organizer with ID {id} not found." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Activate/{id}")]
        public async Task<IActionResult> ActivateOrganizer(int id)
        {
            var result = await _organizerService.ActivateUserAsync(id);

            return result ?
                 Ok(new { message = $"Organizer with ID {id} activated successfully." }) :
                 BadRequest(new { message = $"Failed to activate organizer with ID {id}." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Deactivate/{id}")]
        public async Task<IActionResult> DeactivateOrganizer(int id)
        {
            var result = await _organizerService.DeactivateUserAsync(id);

            return result ?
                 Ok(new { message = $"Organizer with ID {id} deactivated successfully." }) :
                 BadRequest(new { message = $"Failed to deactivate organizer with ID {id}." });
        }

        [Authorize]
        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto updateProfile)
        {
            if (updateProfile == null)
                return BadRequest("Update profile data cannot be null or empty.");

            if (GetAuthenticatedUserId() is not int userId)
                return Unauthorized(new { message = "User ID not found in token." });

            var result = await _organizerService.UpdateUserProfileAsync(userId, updateProfile);

            return result ?
                 Ok(new { message = $"Organizer profile updated successfully." }) :
                 BadRequest(new { message = $"Failed to update organizer profile." });
        }


        [Authorize]
        [HttpPut("UpdateEmail")]
        public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmailDto updateEmail)
        {
            if (updateEmail == null)
                return BadRequest("New email cannot be null or empty.");

            if (GetAuthenticatedUserId() is not int userId)
                return Unauthorized(new { message = "User ID not found in token." });

            await _organizerService.UpdateEmailAsync(userId, updateEmail);

            return Ok(new { message = $"Organizer email updated successfully." });
        }

        [Authorize]
        [HttpPost("password-reset-request")]
        public async Task<IActionResult> PasswordResetRequest()
        {

            if (GetAuthenticatedUserId() is not int userId)
                return Unauthorized(new { message = "User ID not found in token." });

            var user = await _organizerService.GetUserByIdAsync(userId);

            if (user == null)
                return NotFound(new { message = $"User not found." });

            await _verificationService.SendCodeAsync(user, Domain.Enums.enVerificationPurpose.PasswordReset);

            return Ok(new { message = $"Password reset code sent." });
        }

        [Authorize]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (dto == null)
                return BadRequest("Request cannot be null.");

            if (GetAuthenticatedUserId() is not int userId)
                return Unauthorized(new { message = "User ID not found in token." });

            await _organizerService.ResetPassword(userId, dto);

            return Ok(new { message = $"Password reset successfully." });
        }
    }
}

