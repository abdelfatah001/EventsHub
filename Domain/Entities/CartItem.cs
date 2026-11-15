using Domain.Enums;

namespace Domain.Entities
{
    public class CartItem
    {
        public int Id { get; set; }

        public int CartId { get; set; }
        public Cart? Cart { get; set; }

        public int TicketTypeId { get; set; }
        public TicketType? TicketType { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
