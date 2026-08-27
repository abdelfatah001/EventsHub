using Application.Mappers;
using Application.Users.Dto.Request;
using Application.Users.Dto.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;

        private readonly IMapper _mapper;
        public UserService(IUserRepo userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<bool> ActivateUserAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), id, "User id must be a positive integer.");

            var user = await _userRepo.GetByIdAsync(id);

            if (user is null)
                throw new KeyNotFoundException($"User with ID {id} does not exist.");

            if (!user.IsActive)
            {
                user.IsActive = true;
                await _userRepo.UpdateAsync(user);
            }

            return true;
        }

        public async Task<bool> DeactivateUserAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), id, "User id must be a positive integer.");

            var user = await _userRepo.GetByIdAsync(id);

            if (user is null)
                throw new KeyNotFoundException($"User with ID {id} does not exist.");

            if (user.IsActive)
            {
                user.IsActive = false;
                await _userRepo.UpdateAsync(user);
            }

            return true;
        }

        public async Task<ICollection<UserDetailedResponseDto>> GetAllAsync()
        {
            var users = await _userRepo.GetAllAsync();
            return _mapper.Map<ICollection<UserDetailedResponseDto>>(users);
        }

        public async Task<UserDetailedResponseDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), id, "User id must be a positive integer.");

            var user = await _userRepo.GetByIdAsync(id);

            return _mapper.Map<UserDetailedResponseDto?>(user);
        }
        public async Task<UserResponseDto?> GetProfileByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), id, "User id must be a positive integer.");

            var user = await _userRepo.GetByIdAsync(id);

            return _mapper.Map<UserResponseDto?>(user);
        }

        public async Task<bool> UpdateEmailAsync(int id, UpdateEmailDto emailDto)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), id, "User id must be a positive integer.");

            if (emailDto is null)
                throw new ArgumentNullException(nameof(emailDto), "Email update payload cannot be null.");

            var user = await _userRepo.GetByIdAsync(id);

            if (user is null)
                throw new KeyNotFoundException($"User with ID {id} does not exist.");

            if (await _userRepo.ExistsByEmailAsync(emailDto.NewEmail))
                throw new ArgumentException("A user with the same email already exists.", nameof(emailDto.NewEmail));

            // This is a placeholder for the email verification logic.

            user.Email = emailDto.NewEmail;

            return await _userRepo.UpdateAsync(user);

        }

        public async Task<bool> UpdatePasswordAsync(int id, UpdatePasswordDto passwordDto)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), id, "User id must be a positive integer.");

            if (passwordDto is null)
                throw new ArgumentNullException(nameof(passwordDto), "Password update payload cannot be null.");

            var user = await _userRepo.GetByIdAsync(id);

            if (user is null)
                throw new KeyNotFoundException($"User with ID {id} does not exist.");

            if (!BCrypt.Net.BCrypt.Verify(passwordDto.CurrentPassword, user.PasswordHash))
                throw new ArgumentException("Current password is incorrect.", nameof(passwordDto.CurrentPassword));

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordDto.NewPassword);

            return await _userRepo.UpdateAsync(user);
        }

        public async Task<bool> UpdateProfileAsync(int id, UpdateProfileDto profileDto)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), id, "User id must be a positive integer.");

            if (profileDto is null)
                throw new ArgumentNullException(nameof(profileDto), "Profile update payload cannot be null.");

            var user = await _userRepo.GetByIdAsync(id);

            if (user is null)
                throw new KeyNotFoundException($"User with ID {id} does not exist.");

            user.FirstName = profileDto.FirstName;
            user.LastName = profileDto.LastName;
            user.Phone = profileDto.PhoneNumber;

            return await _userRepo.UpdateAsync(user);
        }
    }
}
