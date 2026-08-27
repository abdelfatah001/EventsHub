using Application.Tickets.Dto.Request;
using Domain.Entities;

namespace Application.Tickets
{
    public interface ITicketGenerator
    {
        Task<Ticket> GenerateTicket(IssueTicketDto dto);
    }
}
