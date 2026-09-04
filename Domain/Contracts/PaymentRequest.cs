namespace Domain.Contracts
{
    public class PaymentRequest
    {
        public int OrderId { get; init; }
        public decimal Amount { get; init; }
        public string Currency { get; init; } = "EGP";
        public string CustomerName { get; init; } = string.Empty;
        public string CustomerEmail { get; init; } = string.Empty;
        public string CustomerPhone { get; init; } = string.Empty;
    }
}
