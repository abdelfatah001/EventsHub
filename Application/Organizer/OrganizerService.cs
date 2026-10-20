using Application.Organizers.Dto.Request;
using Application.Organizers.Dto.Response;
using Application.Users;
using Application.Users.Dto.Request;
using Application.Users.Dto.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Organizers
{
    public class OrganizerService : IOrganizerService
    {
        private readonly IOrganizerRepo _organizerRepo;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public OrganizerService (IOrganizerRepo organizerRepo, IUserService userService)
        {
            _organizerRepo= organizerRepo;
            _userService= userService;
        }

        public async Task<bool> ActivateUserAsync(int id)
        {
            var organizer = await _organizerRepo.GetByIdAsync(id);

            if (organizer == null)
                ArgumentNullException.ThrowIfNull(organizer, $"There is no Organizer with Id {id}");

            return await _userService.ActivateUserAsync(organizer.UserId);
        }

        public async Task<Organizer?> AddAsync(AddOrganizerDto organizerDto)
        {
            if (organizerDto == null)
                throw new ArgumentNullException("Organizer paypol can't be null");

            organizerDto.userDto.IsOrganizer = true;
            organizerDto.userDto.Role = Domain.Enums.enUserRole.Organizer;

            var user = await _userService.AddUser(organizerDto.userDto);

            if (user == null)
                throw new BusinessException("Failed to add organizer's user");

            var organizer = new Organizer
            {
                UserId = user.Id,
                User = user,
                OrganizationName = organizerDto.OrganizationName,
                Description = organizerDto.Description
            };

            return await _organizerRepo.AddAsync(organizer);

        }

        public async Task<bool> DeactivateUserAsync(int id)
        {
            var organizer = await _organizerRepo.GetByIdAsync(id);

            if (organizer == null)
                ArgumentNullException.ThrowIfNull(organizer, $"There is no Organizer with Id {id}");

            return await _userService.DeactivateUserAsync(organizer.UserId);
        }

        public async Task<ICollection<OrganizerDetailedResponseDto>> GetAllAsync()
        {
            var organizers = await _organizerRepo.GetAllAsync();
            return _mapper.Map < ICollection<OrganizerDetailedResponseDto>> (organizers);
        }

        public async Task<OrganizerDetailedResponseDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), id, "Organizer id must be a positive integer.");

            var organizer = await _organizerRepo.GetByIdAsync(id);
            return _mapper.Map<OrganizerDetailedResponseDto>(organizer);
        }

        public async Task<OrganizerDetailedResponseDto?> GetByUserIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), id, "User id must be a positive integer.");

            var organizer = await _organizerRepo.GetByUserIdAsync(id);
            return _mapper.Map<OrganizerDetailedResponseDto>(organizer);
        }

        public async Task<OrganizerResponseDto?> GetProfileByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), id, "User id must be a positive integer.");

            var organizer = await _organizerRepo.GetByIdAsync(id);
            return _mapper.Map<OrganizerResponseDto>(organizer);
        }

        public async Task<User> GetUserByIdAsync(int organizerId)
        {
            if (organizerId <= 0)
                throw new ArgumentOutOfRangeException(nameof(organizerId), organizerId, "User id must be a positive integer.");

            return await _organizerRepo.GetUserByIdAsync(organizerId);
        }

        public async Task PasswordResetRequest(int id)
        {
            var organizer = await _organizerRepo.GetByIdAsync(id);

            if (organizer == null)
                throw new KeyNotFoundException($"Orgnaizer with Id {id} does not exist");

            await _userService.PasswordResetRequest(organizer.UserId);
        }

        public async Task ResetPassword(ResetPasswordDto dto)
        {
            await _userService.ResetPassword(dto);
        }

        public async Task UpdateEmailAsync(int id, UpdateEmailDto updateEmail)
        {
            var organizer = await _organizerRepo.GetByIdAsync(id);

            if (organizer == null)
                throw new KeyNotFoundException($"Orgnaizer with Id {id} does not exist");

            await _userService.UpdateEmailAsync(organizer.UserId, updateEmail);
        }

        public async Task<bool> UpdateOrganizerProfileAsync(int id, UpdateOrganizerProfileDto updateProfile)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), id, "Organizer id must be a positive integer.");

            if (updateProfile is null)
                throw new ArgumentNullException(nameof(updateProfile), "Profile update payload cannot be null.");

            var organizer = await _organizerRepo.GetByIdAsync(id);

            if (organizer is null)
                throw new KeyNotFoundException($"Organizer with ID {id} does not exist.");

            organizer.OrganizationName = updateProfile.OrganizationName;
            organizer.Description = updateProfile.Description;

            return await _organizerRepo.UpdateAsync(organizer);

        }

        public async Task<bool> UpdateUserProfileAsync(int id, UpdateProfileDto updateProfile)
        {
            var organizer = await _organizerRepo.GetByIdAsync(id);

            if (organizer == null)
                throw new KeyNotFoundException($"Orgnaizer with Id {id} does not exist");

            return await _userService.UpdateProfileAsync(organizer.UserId, updateProfile);
        }
    }
}
