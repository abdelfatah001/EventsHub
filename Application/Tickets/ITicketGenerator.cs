using Domain.Entities;

namespace Application.Tickets
{
    public interface ITicketGenerator
    {
        Task<Ticket> GenerateTicket(int ticketTypeId,int orderItemId);
    }
}
