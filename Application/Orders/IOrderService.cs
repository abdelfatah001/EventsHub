using Application.OrderItems.Dto.Request;
using Application.Orders.Dto;
using Domain.Contracts;
using Domain.Entities;

namespace Application.Orders
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int userId);
        Task<IEnumerable<GetOrderDto>?> GetOrders (int userId);
        Task<IEnumerable<GetDetailedOrderDto>?> GetUserOrders(int userId);

        Task<Order?> GetByIdAsync(int orderId); 
        Task<OrderSummary?> GetOrderSummaryWithItems(int orderId);
    }
}
