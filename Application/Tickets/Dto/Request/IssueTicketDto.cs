namespace Application.Tickets.Dto.Request
{
    public class IssueTicketDto
    {
        public int OrderItemId { get; set; }
        public int TicketTypeId { get; set; }
    }
}
