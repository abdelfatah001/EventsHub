using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TicketType
    {
        public int Id { get; set; }

        public int OccurenceId { get; set; }
        public EventOccurence? Occurence { get; set; }

        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int AvailableQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = null!;
    }
}
