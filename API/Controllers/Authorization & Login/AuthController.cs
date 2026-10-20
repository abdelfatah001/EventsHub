using API.DTOs.Auth;
using API.DTOs.Auth.Request;
using API.DTOs.Auth.Response;
using Application.Organizers;
using Application.Organizers.Dto.Request;
using Application.Refresh_Token;
using Application.Users;
using Application.Users.Dto.Request;
using Application.Vertification;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOrganizerService _organizerService;
        private readonly JwtSettings _jwtSettings;
        private readonly IRefreshTokenService _tokenService;
        private readonly IVerificationService _verificationService;

        public AuthController(IUserService userService, IOrganizerService organizerService
            , JwtSettings jwtSettings, IRefreshTokenService tokenService,
            IVerificationService verificationService)
        {
            _userService = userService;
            _organizerService = organizerService;
            _jwtSettings = jwtSettings;
            _tokenService = tokenService;
            _verificationService = verificationService;
        }

        private static string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }

        private async Task<TokenResponse> GenerateTokenReponse (User user)
        {
            var Claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            // convert key to bytes
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Key));

            // Hash key by HmacSha256
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: Claims,
                signingCredentials: creds);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            var strRefreshToken = GenerateRefreshToken();

            var RefreshToken = new RefreshToken
            {
                UserId = user.Id,
                TokenHash = BCrypt.Net.BCrypt.HashPassword(strRefreshToken),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                RevokedAt = null
            };

            RefreshToken.Id = await _tokenService.AddAsync(RefreshToken);

            user.ActiveRefreshTokenId = RefreshToken.Id;    

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = strRefreshToken,
            };

        }

        [AllowAnonymous]
        [HttpPost("createAccount")]
        [EnableRateLimiting("AuthLimiter")]
        public async Task<IActionResult> CreateAccount([FromBody]AddUserDto userDto)
        {
            switch (userDto.Role)
            {
                case Domain.Enums.enUserRole.Admin:
                    return BadRequest("Admin account can't be created");

                case Domain.Enums.enUserRole.Organizer:
                {
                    var organizerDto = new AddOrganizerDto
                    {
                        userDto = userDto,
                        OrganizationName = null,
                        Description = null
                    };
                    var organizer = await _organizerService.AddAsync(organizerDto);
                    await _verificationService.SendCodeAsync(organizer.User, Domain.Enums.enVerificationPurpose.EmailVerification);
                    break;
                }

                case Domain.Enums.enUserRole.User:
                {
                    var user = await _userService.AddUser(userDto);
                    await _verificationService.SendCodeAsync(user, Domain.Enums.enVerificationPurpose.EmailVerification);
                    break;
                }

                default:
                    return BadRequest("Invalid user role");
            }

            return Ok("New Account created successfully");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [EnableRateLimiting("AuthLimiter")]
        public async Task<IActionResult> Login([FromBody]LoginRequest request)
        {
            var user = await _userService.GetByEmailAsync(request.Email);

            if (user == null)
                return Unauthorized("Invalid credentials");

            bool isValidPassword =
                 BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isValidPassword)
                return Unauthorized("Invalid Credentials");

            var tokenResponse = await GenerateTokenReponse(user);
            return Ok(tokenResponse);

        }


        [HttpPost("refresh")]
        [EnableRateLimiting("AuthLimiter")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var user = await _userService.GetByEmailAsync(request.Email);

            if (user == null)
                return Unauthorized("Invalid refresh token");

            var refreshToken = await _tokenService.GetActiveTokenByUserIdAsync(user.Id);

            if (refreshToken == null)
                return Unauthorized($"Invalid refresh token");

            if (refreshToken.RevokedAt != null)
                return Unauthorized("Invalid refresh token");

            if (refreshToken.ExpiresAt <= DateTime.UtcNow)
                return Unauthorized("Invalid refresh token");

            bool isValid = BCrypt.Net.BCrypt.Verify(request.RefreshToken, refreshToken.TokenHash);

            if (!isValid)
                return Unauthorized("Invalid refresh token");

            await _tokenService.RevokeAsync(refreshToken);

            var tokenResponse = await GenerateTokenReponse(user);
            return Ok(tokenResponse);

        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut([FromBody] LogoutRequest request)
        {
            var user = await _userService.GetByEmailAsync(request.Email);

            if (user == null)
                return Ok();

            var RefreshToken = await _tokenService.GetActiveTokenByUserIdAsync(user.Id);

            if (RefreshToken == null)
                throw new ArgumentNullException($"Refresh token with Id {user.ActiveRefreshTokenId} is not existed.");

            bool refreshValid = BCrypt.Net.BCrypt.Verify(
                request.RefreshToken, RefreshToken.TokenHash);

            if (!refreshValid)
                return Ok();

            await _tokenService.RevokeAsync(RefreshToken);

            return Ok("Logged out successfully");
        }


    }
}
