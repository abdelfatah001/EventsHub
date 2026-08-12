using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;

        public UserRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> Add(User user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            _context.Users.Add(user);

            return await _context.SaveChangesAsync() > 0 ? user : throw new Exception("Failed to add user.");
        }

        public async Task<ICollection<User> > GetAll()
        {
            return await _context.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User?> GetById(int id)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));

            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> Update(User user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            
            _context.Users.Update(user);

            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> ExistsByEmail(string email)
        {
            ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));

            return await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == email);
        }
        public async Task<bool> ExistsByPhone(string phone)
        {
            ArgumentException.ThrowIfNullOrEmpty(phone, nameof(phone));

            return await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Phone == phone);
        }
    }
}
