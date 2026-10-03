using Application.Users.Dto.Request;
using Application.Users.Dto.Response;
using Domain.Entities;

namespace Application.Users
{
    /// <summary>
    /// Defines user-related operations exposed by the application layer.
    /// Implementations handle creation, retrieval, update, activation, and deactivation of users.
    /// </summary>
    public interface IUserService
    {

        Task<User?> AddUser(AddUserDto user);

        /// <summary>
        /// Retrieves all users with detailed information.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="UserDetailedResponseDto"/> representing all users.
        /// These DTOs are intended for administrative or internal use and may include sensitive or
        /// internal-only fields not suitable for public exposure.
        /// </returns>
        Task<ICollection<UserDetailedResponseDto>> GetAllAsync();

        /// <summary>
        /// Retrieves detailed information for a user by identifier.
        /// </summary>
        /// <param name="id">The identifier of the user to retrieve.</param>
        /// <returns>
        /// A <see cref="UserDetailedResponseDto"/> containing administrative-level details for the specified user,
        /// or null if the user does not exist.
        /// 
        /// Note: This method is intended for admin/internal use and may expose sensitive or internal-only fields
        /// (for example: roles, permissions, audit metadata). Use with appropriate authorization checks.
        /// </returns>
        Task<UserDetailedResponseDto?> GetByIdAsync(int id);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Retrieves the public profile information for a user by identifier.
        /// </summary>
        /// <param name="id">The identifier of the user whose profile is requested.</param>
        /// <returns>
        /// A <see cref="UserResponseDto"/> representing the user's public/profile information,
        /// or null if the user does not exist.
        ///
        /// Note: This method is intended to return non-sensitive information suitable for public consumption
        /// or user-facing profile views. It should not include admin-only or sensitive fields.
        /// </returns>
        Task<UserResponseDto?> GetProfileByIdAsync(int id);

        /// <summary>
        /// Updates the profile information for the specified user.
        /// </summary>
        /// <param name="id">The identifier of the user to update.</param>
        /// <param name="updateProfile">The DTO containing profile changes to apply.</param>
        /// <returns>
        /// True if the update succeeded; otherwise false.
        /// </returns>
        Task<bool> UpdateProfileAsync(int id, UpdateProfileDto updateProfile);


        Task UpdateEmailAsync(int id, UpdateEmailDto updateEmail);

        Task PasswordResetRequest(int userId);
        Task ResetPassword(ResetPasswordDto dto);

        /// <summary>
        /// Activates the specified user account.
        /// </summary>
        /// <param name="id">The identifier of the user to activate.</param>
        /// <returns>
        /// True if the account was successfully activated; otherwise false.
        /// </returns>
        Task<bool> ActivateUserAsync(int id);

        /// <summary>
        /// Deactivates the specified user account.
        /// </summary>
        /// <param name="id">The identifier of the user to deactivate.</param>
        /// <returns>
        /// True if the account was successfully deactivated; otherwise false.
        /// </returns>
        Task<bool> DeactivateUserAsync(int id);
    }
}
