using Domain.Contracts;

namespace Domain.Interfaces.PaymentService
{
    public interface IHmacValidator
    {
        bool IsValid(string hmac, PaymobCallback callBack);
    }
}
