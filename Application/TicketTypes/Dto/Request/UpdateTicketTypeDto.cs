using Domain.Entities;

namespace Application.TicketTypes.Dto.Request
{
    public class UpdateTicketTypeDto
    {
        public string? Description { get; set; }

        public double Price { get; set; }
        public int Quantity { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
