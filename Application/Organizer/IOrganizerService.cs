using Application.Organizers.Dto.Request;
using Application.Organizers.Dto.Response;
using Application.Users.Dto.Request;
using Application.Users.Dto.Response;
using Domain.Entities;

namespace Application.Organizers
{
    public interface IOrganizerService
    {
        Task<Organizer?> AddAsync(AddOrganizerDto user);
        Task<ICollection<OrganizerDetailedResponseDto>> GetAllAsync();
        Task<OrganizerDetailedResponseDto?> GetByIdAsync(int id);
        Task<OrganizerDetailedResponseDto?> GetByUserIdAsync(int id);
        Task<User> GetUserByIdAsync(int organizerId);
        Task<OrganizerResponseDto?> GetProfileByIdAsync(int id);
        Task<bool> UpdateOrganizerProfileAsync(int id, UpdateOrganizerProfileDto updateProfile);

        Task<bool> UpdateUserProfileAsync(int id, UpdateProfileDto updateProfile);
        Task UpdateEmailAsync(int id, UpdateEmailDto updateEmail);

        Task<bool> ActivateUserAsync(int id);
        Task<bool> DeactivateUserAsync(int id);

        Task PasswordResetRequest(int userId);
        Task ResetPassword(ResetPasswordDto dto);

    }
}
