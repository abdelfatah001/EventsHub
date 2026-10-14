using Microsoft.AspNetCore.Mvc;
using Application.Users;
using Application.Users.Dto.Request;
using Application.ResetPasswordToken;
using Application.Vertification;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
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
        [HttpGet("/GetAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllAsync();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/GetById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.GetByIdAsync(id);

            return result is not null ?
                Ok(result) :
                NotFound(new { message = $"User with ID {id} not found." });
        }


        [HttpGet("/GetProfileById/{id}")]
        public async Task<IActionResult> GetUserProfileById(int id)
        {
            var result = await _userService.GetProfileByIdAsync(id);

            return result is not null ?
                Ok(result) :
                NotFound(new { message = $"User with ID {id} not found." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("/Activate/{id}")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var result = await _userService.ActivateUserAsync(id);

            return result ?
                 Ok(new { message = $"User with ID {id} activated successfully." }) :
                 BadRequest(new { message = $"Failed to activate user with ID {id}." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("/Deactivate/{id}")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            var result = await _userService.DeactivateUserAsync(id);

            return result ?
                 Ok(new { message = $"User with ID {id} deactivated successfully." }) :
                 BadRequest(new { message = $"Failed to deactivate user with ID {id}." });
        }

        // owner user
        [HttpPut("/UpdateProfile/{id}")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] UpdateProfileDto updateProfile)
        {
            var result = await _userService.UpdateProfileAsync(id, updateProfile);

            return result ?
                 Ok(new { message = $"User profile with ID {id} updated successfully." }) :
                 BadRequest(new { message = $"Failed to update user profile with ID {id}." });
        }
       
        
        // owner user
        [HttpPut("/UpdateEmail/{id}")]
        public async Task<IActionResult> UpdateEmail(int id, [FromBody] UpdateEmailDto updateEmail)
        {
            await _userService.UpdateEmailAsync(id, updateEmail);

            return Ok(new { message = $"User email with ID {id} updated successfully." });
        }
        
        // owner user
        [HttpPost("password-reset-request")]
        public async Task<IActionResult> PasswordResetRequest(int userId)
        {
            if (userId <= 0)
                return BadRequest(new { message = "Invalid user ID." });

            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
                return NotFound(new { message = $"User with ID {userId} not found." });

            await _verificationService.SendCodeAsync(user, Domain.Enums.enVerificationPurpose.PasswordReset);

            return Ok(new { message = $"Password reset code sent to user with ID {userId}." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (dto == null)
                return BadRequest("Request cannot be null.");

            await _userService.ResetPassword(dto);

            return Ok(new { message = $"Password reset successfully for user with ID {dto.UserId}." });
        }
    }
}