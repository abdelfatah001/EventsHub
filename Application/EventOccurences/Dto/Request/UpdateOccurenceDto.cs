
using Domain.Enums;

namespace Application.EventOccurences.Dto.Request
{
    public class UpdateOccurenceDto
    {
        public int HallId { get; set; }
        public enEventStatus Status { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
    }
}
