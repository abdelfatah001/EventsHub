
namespace Domain.Enums
{
    public enum enPaymentStatus
    {
        Pending = 0, // waiting payment to be completed by the user
        Succeed = 1,
        Failed = 2,
        Refunded = 3 // if expired or canceled
    }
}
