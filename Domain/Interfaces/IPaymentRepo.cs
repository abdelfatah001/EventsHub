using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces
{
    public interface IPaymentRepo
    {
        Task<Payment> CreatePaymentAsync(Payment payment);
        Task<Payment?> GetPaymentByIdAsync(int paymentId);
        Task<Payment?> GetPaymentByProviderReferenceAsync(string providerReference);

        Task<IEnumerable<Payment>?> GetPaymentsByOrderIdAsync(int orderId);
        Task UpdatePaymentStatusAsync(int paymentId, enPaymentStatus status);
    }
}
