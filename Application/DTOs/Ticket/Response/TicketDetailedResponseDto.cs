using Domain.Enums;

namespace Application.DTOs.Ticket.Response
{
    public class TicketDetailedResponseDto
    {
        public string TicketCode { get; set; } = null!;
        public enTicketStatus status { get; set; }
        public DateTime IssuedAt { get; set; }

        public double Price { get; set; }
        public string TicketTypeName { get; set; } = null!;

    }
}
