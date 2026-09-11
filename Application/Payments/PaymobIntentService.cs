using Domain.Contracts;
using Domain.Interfaces;
using Domain.Interfaces.PaymentService;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Payments
{
    public class PaymobIntentService
    {
        private readonly IPaymentRepo _paymentRepo;
        private readonly IPaymentService _paymentService;
        private readonly IUnitOfWork _unitOfWork;

        public PaymobIntentService(IPaymentRepo paymentRepo, IPaymentService paymentService, IUnitOfWork unitOfWork)
        {
            _paymentRepo = paymentRepo;
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Creates a local pending payment record, initiates a payment intent with
        /// the configured payment provider, and stores the provider reference for
        /// later webhook-based payment verification.
        /// </summary>
        public async Task<PaymentResult> CreatePaymentIntentAsync(PaymentRequest paymentRequest)
        {

            var payment = new Payment
            {
                OrderId = paymentRequest.OrderId,
                Amount = paymentRequest.Amount,
                Currency = paymentRequest.Currency,
                Status = Domain.Enums.enPaymentStatus.Pending,
                Provider = "Paymob",
                ProviderReference = null,
                TransactionId = null, // TODO: Set the transaction ID if available
                CreatedAt = DateTime.UtcNow
            };
            
            await _paymentRepo.CreatePaymentAsync(payment);

            await _unitOfWork.SaveChangesAsync();


            var paymentIntent = await _paymentService.CreatePaymentAsync(paymentRequest);

            if (paymentIntent == null)
            {
                payment.Status = Domain.Enums.enPaymentStatus.Failed;

                await _unitOfWork.SaveChangesAsync();

                throw new BusinessException("Failed to create payment intent.");
            }

            payment.ProviderReference = paymentIntent.ProviderReference;



            await _unitOfWork.SaveChangesAsync();
                
            return paymentIntent;
        }
    }
}
