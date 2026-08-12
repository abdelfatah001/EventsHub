
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepo
    {

        Task<User> Add(User user);
        Task<ICollection<User>> GetAll();
        Task<User?> GetById(int id);
        Task<bool> Update(User user);
        Task<bool> ExistsByEmail(string email);
        Task<bool> ExistsByPhone(string phone);
    }
}
