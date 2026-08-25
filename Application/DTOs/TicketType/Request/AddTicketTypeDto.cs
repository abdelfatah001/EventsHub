using Domain.Entities;

namespace Application.DTOs.TicketType.Request
{
    public class AddTicketTypeDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public int OccurenceId { get; set; }

        public double Price { get; set; }
        public int Quantity { get; set; }
        public int AvaliableQuantity { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
