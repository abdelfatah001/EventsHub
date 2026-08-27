using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IOrderRepo
    {
        Task<Order?> AddAsync (Order order);
        Task<IEnumerable<Order>?> GetAllByUserIdAsync (int userId);
        Task<Order?> GetByIdAsync (int orderId);
    }
}
