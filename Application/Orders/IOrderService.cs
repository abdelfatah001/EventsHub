using Application.OrderItems.Dto.Request;
using Domain.Entities;

namespace Application.Orders
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int userId);
    }
}
