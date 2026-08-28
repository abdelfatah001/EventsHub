using Application.OrderItems.Dto.Request;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Enums;
using Domain.Exceptions;
using Application.Carts;


namespace Application.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepository;
        private readonly IOrderItemRepo _orderItemRepository;
        private readonly ITicketTypeRepo _ticketTypeRepository;
        private readonly ITicketRepo _ticketRepository;
        private readonly ICartService _cartService;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(
            IOrderRepo orderRepository, IOrderItemRepo orderItemRepository,
            ITicketTypeRepo ticketTypeRepository, ITicketRepo ticketRepository,
            ICartService cartService, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _ticketTypeRepository = ticketTypeRepository;
            _ticketRepository = ticketRepository;
            _cartService = cartService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Order> CreateOrderAsync(int userId)
        {
            var cart = await _cartService
                .GetCurrentCartByUserIdAsync(userId);

            if (cart is null)
                throw new BusinessException("User does not have an active cart.");

            if (!cart.CartItems.Any())
                throw new BusinessException("Cart is empty.");

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var order = new Order
                {
                    UserId = userId,
                    Status = enOrderStatus.PendingPayment,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                    TotalAmount = 0
                };

                _orderRepository.Add(order);

                foreach (var cartItem in cart.CartItems)
                {
                    // Reserve inventory atomically
                    var reserved = await _ticketTypeRepository
                        .TryReserveAsync(
                            cartItem.TicketTypeId,
                            cartItem.Quantity);

                    if (!reserved)
                        throw new NotEnoughTicketException("Not enough tickets available.");

                    var orderItem = new OrderItem
                    {
                        TicketTypeId = cartItem.TicketTypeId,
                        Quantity = cartItem.Quantity,

                        // TODO : later we can improve this
                        // Price can be changed
                        UnitPrice = await _ticketTypeRepository.GetPriceAsync(cartItem.TicketTypeId), 
                    };

                    order.OrderItems.Add(orderItem);
                    order.TotalAmount += (orderItem.UnitPrice * orderItem.Quantity);

                    _orderItemRepository.Add(orderItem);

                }

                await _cartService.MarkCartAsOrdered(cart.Id);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                return order;

            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }

}

