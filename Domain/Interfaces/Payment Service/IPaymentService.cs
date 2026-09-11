using Domain.Contracts;
using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces.PaymentService
{
    public interface IPaymentService
    {
        Task<PaymentResult> CreatePaymentAsync(PaymentRequest request);
    }
}
