using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IOrderItemRepo
    {
        OrderItem Add(OrderItem item);
        Task<OrderItem?> GetByIdAsync(int itemId);
        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId);
    }
}
