using Application.ResetPasswordToken;
using Application.Users.Dto.Request;
using Application.Users.Dto.Response;
using Application.Vertification;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;


namespace Application.Users
{
    public partial class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IPasswordResetTokenService _resetTokenService;
        private readonly IVerificationService _verificationService;

        public UserService(IUserRepo userRepo, IMapper mapper,
            IVerificationService verificationService, IPasswordResetTokenService resetTokenService)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _verificationService = verificationService;
            _resetTokenService = resetTokenService;
        }

        public async Task<User?> AddUser(AddUserDto userDto)
        {
            if (userDto == null)
                throw new ArgumentNullException("User paypol can't be null");

            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Phone = userDto.Phone,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                Role = userDto.Role,
                IsActive = true,
                CurrentCart = null,
                ActiveRefreshTokenId = null,
                IsOrganizer = (userDto.Role == Domain.Enums.enUserRole.Organizer)
            };

            return await _userRepo.AddAsync(user);
        }
        public async Task<User?> AddAdmin(AddAdminDto admin)
        {
            if(admin == null)
                throw new ArgumentNullException("User paypol can't be null");

            var user = new User
            {
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Email = admin.Email,
                Phone = admin.Phone,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(admin.Password),
                Role = Domain.Enums.enUserRole.Admin,
                IsActive = true,
                CurrentCart = null,
                ActiveRefreshTokenId = null,
                IsOrganizer = false
            };

            return await _userRepo.AddAsync(user);
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

        public async Task<User?> GetByEmailAsync(string email)
        {
            if (email is null)
                throw new ArgumentNullException(nameof(email), "Password update payload cannot be null.");

            var user = await _userRepo.GetByEmailAsync(email);

            return user;
        }

        public async Task<UserResponseDto?> GetProfileByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), id, "User id must be a positive integer.");

            var user = await _userRepo.GetByIdAsync(id);

            return _mapper.Map<UserResponseDto?>(user);
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

        public async Task<User?> GetUserByIdAsync(int id)
        {
            if (id <= 0)
                throw new KeyNotFoundException($"User with Id {id} does not exist");

            var user = await _userRepo.GetByIdAsync(id);

            return user;
        }

        public async Task UpdateEmailAsync(int id, UpdateEmailDto updateEmail)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), id, "User id must be a positive integer.");

            if (updateEmail is null)
                throw new ArgumentNullException(nameof(updateEmail), "Email update payload cannot be null.");

            var user = await _userRepo.GetByIdAsync(id);

            if (user is null)
                throw new KeyNotFoundException($"User with ID {id} does not exist.");

            user.Email = updateEmail.NewEmail;
            user.IsEmailVerified = false; // Reset email verification status

            await _userRepo.UpdateAsync(user);

        }

    }
}
