using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<User> Add(User user);
        Task<ICollection<User>> GetAll();
        Task<User?> GetById(int id);
        Task<bool> UpdateProfile(User user);
        Task<bool> UpdateEmail(int id, string newEmail);
        Task<bool>UpdatePassword(int id, string newPassword);
        Task<bool> ActivateUser(int id);
        Task<bool> DeactivateUser(int id);
    }
}
