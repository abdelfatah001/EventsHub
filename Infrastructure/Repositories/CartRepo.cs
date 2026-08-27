using Domain.Entities;
using Domain.Interfaces;
using Domain.Enums;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CartRepo : ICartRepo
    {
        private readonly AppDbContext _context;

        public CartRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cart?> AddAsync(Cart cart)
        {
            ArgumentNullException.ThrowIfNull(cart, nameof(cart));

            _context.Carts.Add(cart);

            return await _context.SaveChangesAsync() > 0 ? cart : null;
        }

        public async Task<Cart?> GetByIdAsync(int cartId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(cartId);

            return await _context.Carts
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == cartId);
        }

        public async Task<Cart?> GetCurrentCartByUserIdAsync(int userId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);

            return await _context.Carts
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == enCartStatus.Active);
              
        }
    }
}
