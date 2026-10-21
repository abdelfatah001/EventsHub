using Microsoft.AspNetCore.Mvc;
using Application.Users;
using Application.Users.Dto.Request;
using Application.ResetPasswordToken;
using Application.Vertification;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers.Auth
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IVerificationService _verificationService;
        private readonly IPasswordResetTokenService _passwordResetService;

        public UserController(IUserService userService, IVerificationService verificationService, IPasswordResetTokenService passwordResetService)
        {
            _userService = userService;
            _verificationService = verificationService;
            _passwordResetService = passwordResetService;
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllAsync();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.GetByIdAsync(id);

            return result is not null ?
                Ok(result) :
                NotFound(new { message = $"User with ID {id} not found." });
        }

        [Authorize]
        [HttpGet("GetProfileById/{id}")]
        public async Task<IActionResult> GetUserProfileById(int id)
        {
            var result = await _userService.GetProfileByIdAsync(id);

            return result is not null ?
                Ok(result) :
                NotFound(new { message = $"User with ID {id} not found." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Activate/{id}")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var result = await _userService.ActivateUserAsync(id);

            return result ?
                 Ok(new { message = $"User with ID {id} activated successfully." }) :
                 BadRequest(new { message = $"Failed to activate user with ID {id}." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Deactivate/{id}")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            var result = await _userService.DeactivateUserAsync(id);

            return result ?
                 Ok(new { message = $"User with ID {id} deactivated successfully." }) :
                 BadRequest(new { message = $"Failed to deactivate user with ID {id}." });
        }

        [Authorize]
        [HttpPut("UpdateProfile/{id}")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] UpdateProfileDto updateProfile)
        {
            if(updateProfile == null)
                return BadRequest(new { message = "Update profile data cannot be null or empty." });

            if (GetAuthenticatedUserId() is not int userId)
                return Unauthorized(new { message = "User ID not found in token." });

            var result = await _userService.UpdateProfileAsync(userId, updateProfile);

            return result ?
                 Ok(new { message = $"User profile updated successfully." }) :
                 BadRequest(new { message = $"Failed to update user profile." });
        }

        [Authorize]       
        [HttpPut("UpdateEmail/{id}")]
        public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmailDto updateEmail)
        {
            if(updateEmail == null)
                return BadRequest(new { message = "New email cannot be null or empty." });

            if (GetAuthenticatedUserId() is not int userId)
                return Unauthorized(new { message = "User ID not found in token." });

            await _userService.UpdateEmailAsync(userId, updateEmail);

            return Ok(new { message = $"User email updated successfully." });
        }

        [Authorize]
        [HttpPost("password-reset-request")]
        public async Task<IActionResult> PasswordResetRequest()
        {
            if(GetAuthenticatedUserId() is not int userId)
                return Unauthorized(new { message = "User ID not found in token." });

            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
                return NotFound(new { message = $"User with ID {userId} not found." });

            await _verificationService.SendCodeAsync(user, Domain.Enums.enVerificationPurpose.PasswordReset);

            return Ok(new { message = $"Password reset code sent to user with ID {userId}." });
        }

        [Authorize]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (dto == null)
                return BadRequest("Request cannot be null.");

            if (GetAuthenticatedUserId() is not int userId)
                return Unauthorized(new { message = "User ID not found in token." });

            await _userService.ResetPassword(userId, dto);

            return Ok(new { message = $"Password reset successfully." });
        }
    }
}