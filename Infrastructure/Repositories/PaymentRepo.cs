using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class PaymentRepo : IPaymentRepo
    {
        public Task<Payment> CreatePaymentAsync(Payment payment)
        {
            throw new NotImplementedException();
        }

        public Task<Payment?> GetPaymentByIdAsync(int paymentId)
        {
            throw new NotImplementedException();
        }

        public Task<Payment?> GetPaymentByProviderReferenceAsync(string providerReference)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Payment>?> GetPaymentsByOrderIdAsync(int orderId)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePaymentStatusAsync(int paymentId, enPaymentStatus status)
        {
            throw new NotImplementedException();
        }
    }
}
