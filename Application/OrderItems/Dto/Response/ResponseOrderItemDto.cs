using Domain.Entities;

namespace Application.OrderItems.Dto.Response
{
    public class ResponseOrderItemDto
    {
        public Ticket Ticket { get; set; } = null!;

        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
    }
}
