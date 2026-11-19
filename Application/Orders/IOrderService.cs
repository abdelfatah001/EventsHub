
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

        Task<Order> GetOrderWithItemsByIdAsync(int orderId);
        Task<OrderDto?> GetByIdAsync(int orderId); 
        Task<OrderSummary?> GetOrderSummaryWithItems(int orderId);
        Task<ItemDto> GetItemByIdAsync(int orderItemId);
        Task<IEnumerable<ItemDto>?> GetOrderItemsByOrderIdAsync(int orderId);

        Task<bool> IsOrderOwnedByUserAsync(int orderId, int userId);

    }
}
