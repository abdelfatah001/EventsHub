using Application.OrderItems.Dto.Request;
using Domain.Contracts;
using Domain.Entities;

namespace Application.Orders
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int userId);
        Task<IEnumerable<Order>?> GetAllByUserIdAsync(int userId);
        Task<Order?> GetByIdAsync(int orderId); 
        Task<Order?> GetOrderWithItemsByIdAsync(int orderId);
    }
}
