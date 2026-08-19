
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepo
    {

        Task<User?> AddAsync(User user);
        Task<ICollection<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(User user);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByPhoneAsync(string phone);
    }
}
