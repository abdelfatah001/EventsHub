using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces
{
    public interface IPaymentRepo
    {
        Task<Payment> AddPaymentAsync(Payment payment);
        Task<Payment?> GetPaymentByIdAsync(int paymentId);
        Task<Payment?> GetPaymentByProviderReferenceAsync(string providerReference);

        Task<IEnumerable<Payment>?> GetPaymentsByOrderIdAsync(int orderId);
        Task<bool> UpdatePaymentStatusAsync(int paymentId, enPaymentStatus status);
    }
}
