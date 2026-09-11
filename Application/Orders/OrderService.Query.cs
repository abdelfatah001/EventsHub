using Domain.Entities;

namespace Application.Orders
{
    public partial class OrderService
    {
        public Task<IEnumerable<Order>?> GetAllByUserIdAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId), userId, "User id must be a positive integer.");

            return _orderRepository.GetAllByUserIdAsync(userId);

        }
        public Task<Order?> GetByIdAsync(int orderId)
        {
            if (orderId <= 0)
                throw new ArgumentOutOfRangeException(nameof(orderId), orderId, "Order id must be a positive integer.");

            return _orderRepository.GetByIdAsync(orderId);
        }
        public Task<Order?> GetOrderWithItemsByIdAsync(int orderId)
        {
            if (orderId <= 0)
                throw new ArgumentOutOfRangeException(nameof(orderId), orderId, "Order id must be a positive integer.");

            return _orderRepository.GetDetailedOrderByIdAsync(orderId);
        }
    }
}
