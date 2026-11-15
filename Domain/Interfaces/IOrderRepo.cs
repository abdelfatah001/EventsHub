using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IOrderRepo
    {
        Order Add(Order order);
        void Update(Order order);
        Task<IEnumerable<Order>?> GetOrderByUserIdAsync (int userId);
        Task<Order?> GetByIdAsync (int orderId);
        Task<Order?> GetDetailedOrderByIdAsync (int orderId);

    }
}
