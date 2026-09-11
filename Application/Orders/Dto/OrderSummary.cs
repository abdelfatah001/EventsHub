using Application.OrderItems.Dto.Response;

namespace Application.Orders.Dto
{
    public class OrderSummary
    {
        public int Id { get; set; }
        public double TotalAmount { get; set; }
        public IEnumerable<OrderItemSummary> OrderItems { get; set; } = null!;
    }
}
