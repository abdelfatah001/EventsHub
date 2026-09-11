using Domain.Contracts;

namespace Application.Orders.Dto
{
    public class CheckoutResult
    {
        public bool IsSuccess { get; set; }
        public OrderSummary? OrderSummary { get; set; } 
        public PaymentResult? PaymentResult { get; set; }
    }
}
