using Domain.Entities;

namespace Application.OrderItems.Dto.Request
{

    // this is not for user
    public class AddOrderItemDto
    {
        public int TicketTypeId { get; set; }

        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
