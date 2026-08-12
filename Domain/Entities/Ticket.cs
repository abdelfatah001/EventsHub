

using Domain.Enums;
using System;

namespace Domain.Entities
{
    public class Ticket
    {
        public int Id { get; set; }

        public int OrderItemId { get; set; }
        public OrderItem? OrderItem { get; set; }


        public int TicketTypeId { get; set; }
        public TicketType TicketType { get; set; } = null!;
        
        public string TicketCode { get; set; } = null!;
        public enTicketStatus Status { get; set; }

        public DateTime IssuedAt { get; set; }

    }
}
