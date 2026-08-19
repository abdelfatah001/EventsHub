using Application.DTOs.Email;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("/SendEmail")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailDto emailDto)
        {
            await _emailService.SendAsync(emailDto);
            // Logic to send email
            return Ok(new { message = $"Email sent to {emailDto.To} successfully." });
        }

    }
}
