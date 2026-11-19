using Application.Orders.Dto;
using Domain.Entities;

namespace Application.Orders
{
    public partial class OrderService
    {
        public async Task<IEnumerable<GetOrderDto>?> GetOrders(int userId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId), userId, "User id must be a positive integer.");

            var orders = await _orderRepository.GetOrderByUserIdAsync(userId);

            return _mapper.Map<IEnumerable<GetOrderDto>>(orders);

        }
        public async Task<IEnumerable<GetDetailedOrderDto>?> GetUserOrders(int userId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId), userId, "User id must be a positive integer.");

            var orders = await _orderRepository.GetOrderByUserIdAsync(userId);

            return _mapper.Map<IEnumerable<GetDetailedOrderDto>>(orders);

        }


        public async Task<OrderDto?> GetByIdAsync(int orderId)
        {
            if (orderId <= 0)
                throw new ArgumentOutOfRangeException(nameof(orderId), orderId, "Order id must be a positive integer.");

            var order = await _orderRepository.GetByIdAsync(orderId);

            return _mapper.Map<OrderDto>(order);
        }
        public async Task<OrderSummary?> GetOrderSummaryWithItems(int orderId)
        {
            if (orderId <= 0)
                throw new ArgumentOutOfRangeException(nameof(orderId), orderId, "Order id must be a positive integer.");

            var order = await _orderRepository.GetDetailedOrderByIdAsync(orderId);

            return _mapper.Map<OrderSummary>(order);
        }

        public async Task<ItemDto> GetItemByIdAsync(int orderItemId)
        {
            if (orderItemId <= 0)
                throw new ArgumentOutOfRangeException(nameof(orderItemId), orderItemId, "Order item id must be a positive integer.");

            var orderItem = await _orderItemRepository.GetByIdAsync(orderItemId);

            return _mapper.Map<ItemDto>(orderItem);

        }
        public async Task<IEnumerable<ItemDto>?> GetOrderItemsByOrderIdAsync(int orderId)
        {
            if (orderId <= 0)
                throw new ArgumentOutOfRangeException(nameof(orderId), orderId, "Order id must be a positive integer.");


            var orderItem = await _orderItemRepository.GetByOrderIdAsync(orderId);

            return _mapper.Map<IEnumerable<ItemDto>>(orderItem);
        }

        public async Task<bool> IsOrderOwnedByUserAsync(int orderId, int userId)
        {
            if (orderId <= 0)
                throw new ArgumentOutOfRangeException(nameof(orderId), orderId, "Order id must be a positive integer.");

            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId), userId, "User id must be a positive integer.");

            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order is null)
                return false;

            return order.UserId == userId;

        }
         public async Task<Order> GetOrderWithItemsByIdAsync(int orderId)
        {

            if (orderId <= 0)
                throw new ArgumentOutOfRangeException(nameof(orderId), orderId, "Order id must be a positive integer.");


            return await _orderRepository.GetDetailedOrderByIdAsync(orderId);

        }

    }
}
