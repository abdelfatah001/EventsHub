using Application.Tickets.Dto.Response;

namespace Application.OrderItems.Dto.Response
{
    public class OrderItemSummary
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

    }
}
