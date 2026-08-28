using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;
namespace Infrastructure.Repositories
{
    public class CartItemRepo : ICartItemRepo
    {

        private readonly AppDbContext _context;

        public CartItemRepo (AppDbContext context)
        {
            _context = context;
        }

        public async Task<CartItem?> AddAsync(CartItem item)
        {
            ArgumentNullException.ThrowIfNull(item, nameof(item));

            _context.CartItems.Add(item);

            return await _context.SaveChangesAsync() > 0 ? item : null;
        }

        

        public async Task<IEnumerable<CartItem>> GetByCartId(int cartId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(cartId);

            return await _context.CartItems
                .AsNoTracking()
                .Where(c => c.Id == cartId)
                .ToListAsync();
        }

        public async Task<CartItem?> GetById(int cartItemId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(cartItemId);

            return await _context.CartItems
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == cartItemId);
        }

        public async Task<int> IsCartItemRepeated(int cartId, int TicketTypeId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(cartId);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(TicketTypeId);

            return await _context.CartItems
                .Where(x => 
                x.CartId == cartId &&
                x.TicketTypeId == TicketTypeId &&
                x.Cart.Status == enCartStatus.Active) // it makes inner join with cart
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            // if no existed 0 will returned

        }

        public async Task<bool> UpdateAsync(CartItem cartItem)
        {
            ArgumentNullException.ThrowIfNull(cartItem, nameof(cartItem));

            _context.CartItems.Update(cartItem);

            return await _context.SaveChangesAsync() > 0;

        }
        public async Task<int> CalculateCartTotalAmount(int cartId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(cartId);

            return await _context.CartItems
                .Where(x => x.CartId == cartId)
                .SumAsync(x => x.Quantity * x.UnitPrice);
        }
        
        public async Task<bool> DeleteAsync(CartItem cartItem)
        {
            ArgumentNullException.ThrowIfNull(cartItem, nameof(cartItem));

            _context.CartItems.Remove(cartItem);

            return await _context.SaveChangesAsync() > 0;
        }

    }
}
