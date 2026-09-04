using Domain.Contracts;
using Domain.Entities;

namespace Domain.Interfaces.Payment
{
    public interface IPaymentRepo
    {
        Task<PaymentResult> CreatePaymentAsync(PaymentRequest request);
    }
}
