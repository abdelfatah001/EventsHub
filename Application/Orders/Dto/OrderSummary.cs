using Application.OrderItems.Dto.Response;

namespace Application.Orders.Dto
{
    public class OrderSummary
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; } 
        public IEnumerable<OrderItemSummary> OrderItems { get; set; } = null!;
    }
}
