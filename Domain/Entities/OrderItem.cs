

namespace Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public ICollection<int> TicketIds { get; set; }
        public ICollection<Ticket> Tickets { get; set; }

        public int TicketTypeId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

    }
}
