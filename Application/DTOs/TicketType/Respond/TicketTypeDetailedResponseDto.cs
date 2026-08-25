using Domain.Entities;

namespace Application.DTOs.TicketType.Respond
{
    public class TicketTypeDetailedResponseDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public int Price { get; set; }
        public int Quantity { get; set; }
        public int AvailableQuantity { get; set; }

        public int OccurenceId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
