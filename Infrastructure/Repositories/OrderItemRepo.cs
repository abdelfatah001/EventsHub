using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DbContexts;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrderItemRepo : IOrderItemRepo
    {
        private readonly AppDbContext _context;

        public OrderItemRepo(AppDbContext context)
        {
            _context = context;
        }

        public OrderItem AddA(OrderItem item)
        {
            ArgumentNullException.ThrowIfNull(item, nameof(item));

            _context.OrderItems.Add(item);

            return item;
        }

        public async Task<OrderItem?> GetById(int itemId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(itemId);

            return await _context.OrderItems
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == itemId);
        }

        public async Task<IEnumerable<OrderItem>> GetByOrderId(int orderId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(orderId);

            return await _context.OrderItems
                .AsNoTracking()
                .Where(c => c.OrderId == orderId)
                .ToListAsync();
        }
    }
}
