using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IOrderRepo
    {
        Order Add(Order order);
        Task<IEnumerable<Order>?> GetAllByUserIdAsync (int userId);
        Task<Order?> GetByIdAsync (int orderId);
        Task<Order?> GetDetailedOrderByIdAsync (int orderId);

    }
}
