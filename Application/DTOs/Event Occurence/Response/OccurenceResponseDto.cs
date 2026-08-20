using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs.EventOccurences.Response
{
    public class OccurenceResponseDto
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public string Location { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public enEventStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
