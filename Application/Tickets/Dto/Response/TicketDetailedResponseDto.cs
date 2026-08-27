using Domain.Enums;

namespace Application.Tickets.Dto.Response
{
    public class TicketDetailedResponseDto
    {
        public string TicketCode { get; set; } = null!;
        public enTicketStatus status { get; set; }
        public DateTime IssuedAt { get; set; }

        public double Price { get; set; }

    }
}
