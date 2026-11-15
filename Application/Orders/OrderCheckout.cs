using Application.OrderItems.Dto.Response;
using Application.Orders.Dto;
using Application.Payments;
using Domain.Contracts;
using Domain.Entities;

namespace Application.Orders
{
    public class OrderCheckout
    {
        private readonly IOrderService _orderService;
        private readonly PaymobIntentService _paymobIntentService;

        public OrderCheckout(IOrderService orderService, PaymobIntentService paymobIntentService)
        {
            _orderService = orderService;
            _paymobIntentService = paymobIntentService;
        }

        /// <summary>
        /// Coordinates the checkout workflow by creating an order from the user's cart,
        /// initiating the payment intent, and returning the order summary and payment
        /// details required to complete the payment.
        /// </summary>
        public async Task<CheckoutResult> CheckoutOrderAsync(User user)
        {
            var order = await _orderService.CreateOrderAsync(user.Id);


            var paymentRequest = new PaymentRequest
            {
                Amount = order.TotalAmount,
                Currency = "EGP",
                OrderId = order.Id,
                CustomerName = user.FirstName + " " + user.LastName, 
                CustomerEmail = user.Email,
                CustomerPhone = user.Phone 
            };

            var paymentResult = await _paymobIntentService.CreatePaymentIntentAsync(paymentRequest);

            return new CheckoutResult
            {
                IsSuccess = true,
                OrderSummary = new OrderSummary
                {
                    Id = order.Id,
                    TotalAmount = order.TotalAmount,
                    OrderItems = order.OrderItems.Select(item => new OrderItemSummary
                    {
                        Id = item.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                    }).ToList()
                },
                PaymentResult = paymentResult
            };

        }
        
    }
}
