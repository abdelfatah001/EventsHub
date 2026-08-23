
using Domain.Enums;

namespace Application.DTOs.EventOccurences.Request
{
    public class UpdateOccurenceDto
    {
        public int HallId { get; set; }
        public enEventStatus Status { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
    }
}
