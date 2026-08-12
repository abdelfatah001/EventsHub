using Domain.Enums;
using System;
using System.Collections.Generic;


namespace Domain.Entities
{
    public class EventOccurence
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; } = null!;
        
        public string Location { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public enEventStatus Status { get; set; }   

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<TicketType> TicketTypes { get; set; } 
            = new List<TicketType>();
    }
}
