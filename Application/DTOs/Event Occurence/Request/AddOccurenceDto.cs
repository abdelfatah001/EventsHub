using Domain.Enums;

namespace Application.DTOs.EventOccurences.Request
{
    public class AddOccurenceDto
    {
        public int EventId { get; set; }
        public int HallId { get; set; }
        public enEventStatus Status { get; set; } 
        public DateTime StartAt { get; set;}
        public DateTime EndAt { get; set; }
    }
}
