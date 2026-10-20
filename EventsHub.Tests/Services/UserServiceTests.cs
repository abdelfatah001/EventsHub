using Application.ResetPasswordToken;
using Application.Users;
using Application.Users.Dto.Request;
using Application.Vertification;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Moq;

namespace EventsHub.Tests.Services
{
    public class UserServiceTests
    {
        private static UserService CreateUserService(Mock<IUserRepo> userRepo)
        {

            var mapper = new Mock<AutoMapper.IMapper>();
            var resedPasswordToken = new Mock<IPasswordResetTokenService>();
            var verificationService = new Mock<IVerificationService>();

            return new UserService(
                userRepo.Object, mapper.Object, verificationService.Object, resedPasswordToken.Object);
        }

        public async Task<(User, Mock<IUserRepo>)> ExecuteAddUser(AddUserDto submittedUser)
        {
            var user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Phone = "01234567890",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
                Role = Domain.Enums.enUserRole.User,
            };

            var userRepo = new Mock<IUserRepo>();

            userRepo
                .Setup(userRepo => userRepo.AddAsync(It.IsAny<User>()))
                .ReturnsAsync((User user) => { user.Id = 1; return user; });

            var userService = CreateUserService(userRepo);

            var result = await userService.AddUser(submittedUser);
            return (result, userRepo);
        }

        [Fact]
        public async Task AddUser_WithValidDto_CreatesUserWithHashedPassword()
        {
            // Arrange
            var userDto = new AddUserDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Phone = "01234567890",
                Password = "password",
                Role = Domain.Enums.enUserRole.User
            };

            // Act
            var (result, userRepo) = await ExecuteAddUser(userDto);

            // Assert
            Assert.NotNull(result);
            Assert.True(BCrypt.Net.BCrypt.Verify(userDto.Password, result.PasswordHash));
            Assert.Equal(userDto.FirstName, result.FirstName);
            Assert.Equal(userDto.LastName, result.LastName);
            Assert.Equal(userDto.Email, result.Email);
            Assert.Equal(userDto.Phone, result.Phone);
            Assert.Equal(userDto.Role, result.Role);
            Assert.True(result.IsActive);
            Assert.False(result.IsOrganizer);

            userRepo.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
            userRepo.Verify(repo => repo.ExistsByEmailAsync(userDto.Email), Times.Once);
            userRepo.Verify(repo => repo.ExistsByPhoneAsync(userDto.Phone), Times.Once);


        }

        [Fact]
        public async Task AddUser_WithDuplicateEmail_ThrowsInvalidOperationException()
        {
            // Arrange
            var userDto = new AddUserDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Phone = "01234567890",
                Password = "password",
                Role = Domain.Enums.enUserRole.User
            };
            var userRepo = new Mock<IUserRepo>();
            userRepo
                .Setup(repo => repo.ExistsByEmailAsync(userDto.Email))
                .ReturnsAsync(true);

            var userService = CreateUserService(userRepo);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => userService.AddUser(userDto));

            userRepo.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
            userRepo.Verify(repo => repo.ExistsByEmailAsync(userDto.Email), Times.Once);
        }

        [Fact]
        public async Task AddUser_WithDuplicatePhone_ThrowsInvalidOperationException()
        {
            // Arrange
            var userDto = new AddUserDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Phone = "01234567890",
                Password = "password",
                Role = Domain.Enums.enUserRole.User
            };

            var userRepo = new Mock<IUserRepo>();
            userRepo
                .Setup(repo => repo.ExistsByPhoneAsync(userDto.Phone))
                .ReturnsAsync(true);

            var userService = CreateUserService(userRepo);



            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => userService.AddUser(userDto));

            userRepo.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
            userRepo.Verify(repo => repo.ExistsByPhoneAsync(userDto.Phone), Times.Once);
        }
      
    }
}
