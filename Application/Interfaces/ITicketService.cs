using Application.DTOs.Ticket.Request;
using Application.DTOs.Ticket.Response;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ITicketService
    {
        Task<TicketResponseDto> AddAsync(IssueTicketDto ticket);

        Task<IEnumerable<TicketDetailedResponseDto>> GetAllByTicketTypeIdAsync(int ticketTypeId);

        Task<IEnumerable<TicketDetailedResponseDto>> GetAllByOrderItemIdAsync(int orderItemId);

        Task<TicketDetailedResponseDto> GetByIdAsync(int ticketId);

        Task<bool> CancelTicket(int ticketId);

        Task<bool> DeactivateTicket (int ticketId); // make it used

    }
}
