

namespace Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int TicketId { get; set; }
        public Ticket? Ticket { get; set; }

        public int TicketTypeId { get; set; }

        public int Quantity { get; set; }
        public double UnitPrice { get; set; }

    }
}
