using Application.Users;
using Application.Users.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdmin([FromBody] AddAdminDto dto)
        {
            var admin = await _userService.AddAdmin(dto);

            return CreatedAtAction(nameof(GetUserById),new { id = admin.Id },admin);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserById([FromBody] int Id)
        {
            var user = await _userService.GetByIdAsync(Id);

            return Ok(user);
        }
    }

}

