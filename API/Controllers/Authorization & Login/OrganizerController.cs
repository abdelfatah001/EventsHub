using Application.Organizers;
using Application.ResetPasswordToken;
using Application.Users;
using Application.Users.Dto.Request;
using Application.Vertification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizerController : Controller
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
        [HttpGet("/GetAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _organizerService.GetAllAsync();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/GetById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _organizerService.GetByIdAsync(id);

            return result is not null ?
                Ok(result) :
                NotFound(new { message = $"Organizer with ID {id} not found." });
        }


        [HttpGet("/GetProfileById/{id}")]
        public async Task<IActionResult> GetUserProfileById(int id)
        {
            var result = await _organizerService.GetProfileByIdAsync(id);

            return result is not null ?
                Ok(result) :
                NotFound(new { message = $"Organizer with ID {id} not found." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("/Activate/{id}")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var result = await _organizerService.ActivateUserAsync(id);

            return result ?
                 Ok(new { message = $"Organizer with ID {id} activated successfully." }) :
                 BadRequest(new { message = $"Failed to activate organizer with ID {id}." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("/Deactivate/{id}")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            var result = await _organizerService.DeactivateUserAsync(id);

            return result ?
                 Ok(new { message = $"Organizer with ID {id} deactivated successfully." }) :
                 BadRequest(new { message = $"Failed to deactivate organizer with ID {id}." });
        }

        // owner user
        [HttpPut("/UpdateProfile/{id}")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] UpdateProfileDto updateProfile)
        {
            var result = await _organizerService.UpdateUserProfileAsync(id, updateProfile);

            return result ?
                 Ok(new { message = $"Organizer profile with ID {id} updated successfully." }) :
                 BadRequest(new { message = $"Failed to update organizer profile with ID {id}." });
        }


        // owner user
        [HttpPut("/UpdateEmail/{id}")]
        public async Task<IActionResult> UpdateEmail(int id, [FromBody] UpdateEmailDto updateEmail)
        {
            await _organizerService.UpdateEmailAsync(id, updateEmail);

            return Ok(new { message = $"Organizer email with ID {id} updated successfully." });
        }

        // owner user
        [HttpPost("password-reset-request")]
        public async Task<IActionResult> PasswordResetRequest(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid user ID." });

            var user = await _organizerService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound(new { message = $"User with ID {user.Id} not found." });

            await _verificationService.SendCodeAsync(user, Domain.Enums.enVerificationPurpose.PasswordReset);

            return Ok(new { message = $"Password reset code sent to user with ID {user.Id}." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (dto == null)
                return BadRequest("Request cannot be null.");

            await _organizerService.ResetPassword(dto);

            return Ok(new { message = $"Password reset successfully for user with ID {dto.UserId}." });
        }
    }
}

