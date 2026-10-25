using Application.Orders;
using Application.Tickets;
using Application.TicketTypes;
using Domain.Contracts;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.PaymentService;

namespace Application.Payments
{
    public class PaymobWebhookService
    {
        private readonly IHmacValidator _hmacValidator;
        private readonly IPaymentRepo _paymentRepo;
        private readonly ITicketService _ticketService;
        private readonly TicketTypeService _ticketTypeService;
        private readonly IOrderService _orderService;
        private readonly IUnitOfWork _unitOfWork;

        public PaymobWebhookService(IHmacValidator hmacValidator, IPaymentRepo paymentRepo, 
            ITicketService ticketService, TicketTypeService ticketTypeService,
            IOrderService orderService, IUnitOfWork unitOfWork)
        {
            _hmacValidator = hmacValidator;
            _paymentRepo = paymentRepo;
            _ticketService = ticketService;
            _ticketTypeService = ticketTypeService;
            _orderService = orderService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processes a Paymob payment webhook by validating its HMAC signature,
        /// ensuring the payment has not already been processed, and atomically
        /// updating the payment and order status. On successful payment, tickets
        /// are issued; otherwise, reserved tickets are released.
        /// </summary>
        public async Task ProcessWebhookAsync(string hmac, PaymobCallback callback, string providerReference)
        {

            // validate the HMAC signature
            bool isValid = _hmacValidator.IsValid(hmac, callback);

            if (!isValid)
                throw new UnauthorizedAccessException("Invalid HMAC signature");

            // Idempotency check : check if payment exists and is already processed before
            var payment = await _paymentRepo.GetPaymentByProviderReferenceAsync(providerReference);

            if (payment == null)
                throw new KeyNotFoundException("Payment not found");

            if (payment.Status == enPaymentStatus.Succeed ||
                payment.Status == enPaymentStatus.Failed ||
                payment.Status == enPaymentStatus.Refunded)
            {
                return;
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var order = await _orderService.GetOrderWithItemsByIdAsync(payment.OrderId);

                if (order == null)
                    throw new KeyNotFoundException("Order not found");

                if (callback.Success)
                {

                    foreach (var item in order.OrderItems)
                    {
                        await _ticketService.AddTicketsAsync(item.TicketTypeId, item.Id, item.Quantity);
                    }

                    order.Status = Domain.Enums.enOrderStatus.Paid;
                    payment.Status = enPaymentStatus.Succeed;

                }
                else
                {
                    payment.Status = enPaymentStatus.Failed;
                    order.Status = Domain.Enums.enOrderStatus.PaymentFailed;

                    foreach (var item in order.OrderItems)
                    {
                        await _ticketTypeService.ReleaseReservedTickets(item.TicketTypeId, item.Quantity);
                    }
                }

                // assign the transaction ID from the callback to the payment record
                payment.TransactionId = callback.Id;

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

            }
            catch 
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw; 
            }
        }
    }
}
