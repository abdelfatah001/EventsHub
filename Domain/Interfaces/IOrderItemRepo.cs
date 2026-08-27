using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IOrderItemRepo
    {
        Task<OrderItem?> AddAsync(OrderItem item);
        Task<OrderItem?> GetById(int itemId);
        Task<IEnumerable<OrderItem>> GetByOrderId(int orderId);
    }
}
