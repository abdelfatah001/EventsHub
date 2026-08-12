using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;

        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public Task<bool> ActivateUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User> Add(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeactivateUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<User>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateEmail(int id, string newEmail)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePassword(int id, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateProfile(User user)
        {
            throw new NotImplementedException();
        }
    }
}
