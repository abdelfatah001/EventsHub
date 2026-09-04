using Domain.Contracts;

namespace Domain.Interfaces.Payment
{
    public interface IHmacValidator
    {
        bool IsValid(string hmac, PaymobCallback callBack);
    }
}
