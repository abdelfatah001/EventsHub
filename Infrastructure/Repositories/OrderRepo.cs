using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrderRepo : IOrderRepo
    {
        private readonly AppDbContext _context;

        public OrderRepo (AppDbContext context)
        {
            _context = context;
        }

        public Order Add(Order order)
        {
            ArgumentNullException.ThrowIfNull(order, nameof(order));
            
            _context.Orders.Add(order);

            return order;
        }

        public async Task<IEnumerable<Order>?> GetAllByUserIdAsync(int userId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);

            return await _context.Orders
                .AsNoTracking()
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int orderId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(orderId);

            return await _context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == orderId);
        }
    }
}
