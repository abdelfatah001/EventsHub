using API.Controllers;
using API.DTOs.Auth;
using Application.Organizers;
using Application.Refresh_Token;
using Application.Users;
using Application.Vertification;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Moq;
using API.DTOs.Auth.Request;
using API.DTOs.Auth.Response;
using API.Controllers.Auth;


namespace EventsHub.Tests.Controllers
{
    public class AuthControllerTests
    {

        private readonly Mock<IUserService> _userService;
        private readonly Mock<IRefreshTokenService> _tokenService;
        
        private readonly Mock<IOrganizerService> _organizerService;
        private readonly Mock<IVerificationService> _verificationService;

        private readonly JwtSettings _jwtSettings;

        private readonly AuthController _authController;

        public AuthControllerTests()
        {
            _userService = new Mock<IUserService>();
            _tokenService = new Mock<IRefreshTokenService>();
            _organizerService = new Mock<IOrganizerService>();
            _verificationService = new Mock<IVerificationService>();

            _jwtSettings = new JwtSettings
            {
                Key = "replace-with-a-test-key-at-least-32-bytes-long",
                Issuer = "EventsHub.Tests",
                Audience = "EventsHub.Tests"
            };

            _authController = new AuthController(
                _userService.Object, _organizerService.Object, _jwtSettings,
                _tokenService.Object, _verificationService.Object); 
        }


       

        private async Task<IActionResult> ExecuteLogin 
            (string submittedEmail, string submittedPassword, bool IsUserExisted = true)
        {
            const string password = "correct_password";

            var user = (IsUserExisted) ?
                new User()
                {

                    FirstName = "fName",
                    LastName = "lName",
                    Phone = "01212334567",
                    Email = "user@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
                } : null;


            _userService
                .Setup(service => service.GetByEmailAsync(submittedEmail))
                .ReturnsAsync(user);


            _tokenService
                .Setup(s => s.AddAsync(It.IsAny<RefreshToken>()))
                .ReturnsAsync(1);


            var request = new LoginRequest
            {
                Email = submittedEmail,
                Password = submittedPassword,
            };

            var result = await _authController.Login(request);

            return (result);

        }

        [Fact]
        public async Task Login_WhenEmailDoesNotExist_ReturnsUnauthorized ()
        {
            // Arrange
            const string email = "unknown@example.com";
            const string password = "password";

            // Act 
            var result = await ExecuteLogin(email, password, false);

            // Assert 
            Assert.IsType<UnauthorizedObjectResult>(result);

            // It ensures that the Login endpoint called GetByEmailAsync(email) exactly once during this test.
            _userService.Verify(
                s => s.GetByEmailAsync(email),
                Times.Once());
        }

        [Fact]
        public async Task Login_WhenPasswordIsWrong_ReturnsUnauthorized ()
        {
            // Arrange
            const string email = "user@example.com";
            const string wrongPassword = "WrongPassword";

            //Act 
            var result = await ExecuteLogin(email, wrongPassword);

            // Assert 
            Assert.IsType<UnauthorizedObjectResult>(result);

            _userService.Verify(
                s => s.GetByEmailAsync(email),
                Times.Once());  

        }

        [Fact]
        public async Task Login_WhenCredentialsAreCorrect_ReturnsTokenResponseWithTokens()
        {
            const string email = "user@example.com";
            const string password = "correct_password";

            // Act 
            var result = await ExecuteLogin(email, password);


            // Assert

            // the result should be of type OkObjectResult, and the value of that result should be of type TokenResponse.
            var okResult = Assert.IsType<OkObjectResult>(result);
            var tokenResponse = Assert.IsType<TokenResponse>(okResult.Value);

            // the AccessToken and RefreshToken properties of the TokenResponse object should not be null or empty.
            Assert.False(string.IsNullOrEmpty(tokenResponse.AccessToken));
            Assert.False(string.IsNullOrEmpty(tokenResponse.RefreshToken));

            _userService.Verify(
                s => s.GetByEmailAsync(email),
                Times.Once);

            _tokenService.Verify(
                s => s.AddAsync(It.IsAny<RefreshToken>()),
                Times.Once);

        }


        private async Task<IActionResult> ExecuteRefresh(
            DateTime expiresAt, DateTime? revokedAt, string submittedToken,
            bool IsUserExisted = true, bool IsTokenExisted = true)
        {
            const string email = "user@example.com";

            var user = (IsUserExisted)?
                new User
                {
                    Id = 1,
                    Email = email
                } : null;

            var storedToken = (IsTokenExisted)?
                new RefreshToken
                {
                    UserId = 1,
                    TokenHash = BCrypt.Net.BCrypt.HashPassword("correct-refresh-token"),
                    ExpiresAt = expiresAt,
                    RevokedAt = revokedAt
                } : null;

            _userService
                .Setup(s => s.GetByEmailAsync(email))
                .ReturnsAsync(user);

            _tokenService
                .Setup(s => s.GetActiveTokenByUserIdAsync(1))
                .ReturnsAsync(storedToken);


            var request = new RefreshTokenRequest
            { 
                Email = email,
                RefreshToken = submittedToken
            };

            var result = await _authController.Refresh(request);
            return (result);
        }

        [Fact]
        public async Task Refresh_WhenTokenIsInvalid_ReturnsUnauthorized ()
        {
            // Arrange
            const string submittedToken = "wrong-refresh-token";


            // Act 
            var result = await ExecuteRefresh(
                DateTime.UtcNow.AddDays(7),
                null, 
                submittedToken);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);

            // cuz is invalid token ensure the code didn't reach to these actions
            _tokenService.Verify(
                s => s.RevokeAsync(It.IsAny<RefreshToken>()),
                Times.Never());

            _tokenService.Verify(
                s => s.AddAsync(It.IsAny<RefreshToken>()),
                Times.Never()); 

        }

        [Fact] 
        public async Task Refresh_WhenTokenIsExpired_ReturnsUnauthorized ()
        {
            // Act
            var result = await ExecuteRefresh(
                DateTime.UtcNow.AddMinutes(-1),
                null,
                "correct-refresh-token");

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);

            _tokenService.Verify(
                s => s.RevokeAsync(It.IsAny<RefreshToken>()),
                Times.Never());

            _tokenService.Verify(
                s => s.AddAsync(It.IsAny<RefreshToken>()),
                Times.Never());
        }

        [Fact]
        public async Task Refresh_WhenTokenIsRevoked_ReturnsUnauthorized ()
        {
            var result = await ExecuteRefresh(
                DateTime.UtcNow.AddDays(7),
                DateTime.UtcNow.AddDays(-1),
                "correct-refresh-token");

            Assert.IsType<UnauthorizedObjectResult>(result);

            _tokenService.Verify(
                s => s.RevokeAsync(It.IsAny<RefreshToken>()), 
                Times.Never());


            _tokenService.Verify(
                s => s.AddAsync(It.IsAny<RefreshToken>()),
                Times.Never());
        }

        [Fact]
        public async Task Refresh_WhenTokenIsValid_ReturnsOkAndCreatesNewToken ()
        {
            var result = await ExecuteRefresh(
                DateTime.UtcNow.AddDays(7),
                null,
                "correct-refresh-token");

            Assert.IsType<OkObjectResult>(result);


            _tokenService.Verify(
                s => s.RevokeAsync(It.IsAny<RefreshToken>()),
                Times.Once());

            _tokenService.Verify(
                s => s.AddAsync(It.IsAny<RefreshToken>()),
                Times.Once());
        }

        [Fact]
        public async Task Refresh_WhenUserDoesNotExist_ReturnsUnauthorized ()
        {
            var result = await ExecuteRefresh(
                DateTime.UtcNow.AddDays(7),
                null,
                "correct-refresh-token",
                false);

            Assert.IsType<UnauthorizedObjectResult>(result);

            _tokenService.Verify(
                s => s.RevokeAsync(It.IsAny<RefreshToken>()),
                Times.Never());
            _tokenService.Verify(
                s => s.AddAsync(It.IsAny<RefreshToken>()),
                Times.Never());
        }

        [Fact]
        public async Task Refresh_WhenTokenDoesNotExist_ReturnsUnauthorized()
        {
            var result = await ExecuteRefresh(
                DateTime.UtcNow.AddDays(7),
                null,
                "correct-refresh-token",
                true, 
                false);

            Assert.IsType<UnauthorizedObjectResult>(result);

            _tokenService.Verify(
                s => s.RevokeAsync(It.IsAny<RefreshToken>()),
                Times.Never());
            _tokenService.Verify(
                s => s.AddAsync(It.IsAny<RefreshToken>()),
                Times.Never());
        }


    }
}
