using Microsoft.AspNetCore.Mvc;
using Application.Users;
using Application.Users.Dto.Request;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }



        // admin
        [HttpGet("/GetAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllAsync();
            return Ok(result);
        }

        // admin
        [HttpGet("/GetById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.GetByIdAsync(id);

            return result is not null ?
                Ok(result) :
                NotFound(new { message = $"User with ID {id} not found." });
        }
        
        // everyone
        [HttpGet("/GetProfileById/{id}")]
        public async Task<IActionResult> GetUserProfileById(int id)
        {
            var result = await _userService.GetProfileByIdAsync(id);

            return result is not null ?
                Ok(result) :
                NotFound(new { message = $"User with ID {id} not found." });
        }

        // admin
        [HttpPut("/Activate/{id}")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var result = await _userService.ActivateUserAsync(id);

            return result ?
                 Ok(new { message = $"User with ID {id} activated successfully." }) :
                 BadRequest(new { message = $"Failed to activate user with ID {id}." });
        }

        // admin
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
            var result = await _userService.UpdateEmailAsync(id, updateEmail);
            return result ?
                 Ok(new { message = $"User email with ID {id} updated successfully." }) :
                 BadRequest(new { message = $"Failed to update user email with ID {id}." });
        }

        // owner user
        [HttpPut("/UpdatePassword/{id}")]
        public async Task<IActionResult> UpdatePassword(int id, [FromBody] UpdatePasswordDto updatePassword)
        {
            var result = await _userService.UpdatePasswordAsync(id, updatePassword);
            return result ?
                 Ok(new { message = $"User password with ID {id} updated successfully." }) :
                 BadRequest(new { message = $"Failed to update user password with ID {id}." });

        }
    }
}
