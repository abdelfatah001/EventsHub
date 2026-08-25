using Domain.Entities;

namespace Application.DTOs.TicketType.Request
{
    public class UpdateTicketTypeDto
    {
        public string? Description { get; set; }

        public double Price { get; set; }
        public int Quantity { get; set; }
        public int AvaliableQuantity { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
