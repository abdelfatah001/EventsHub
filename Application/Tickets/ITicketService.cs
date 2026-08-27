using Application.Tickets.Dto.Request;
using Application.Tickets.Dto.Response;
using Domain.Entities;

namespace Application.Tickets
{
    public interface ITicketService
    {
        Task<TicketResponseDto> AddAsync(IssueTicketDto ticket);

        Task<IEnumerable<TicketDetailedResponseDto>> GetAllByTicketTypeIdAsync(int ticketTypeId);

        Task<IEnumerable<TicketDetailedResponseDto>> GetAllByOrderItemIdAsync(int orderItemId);

        Task<TicketDetailedResponseDto> GetByIdAsync(int ticketId);

        Task<TicketDetailedResponseDto?> GetByTicketCodeAsync(string ticketCode);

        Task<bool> DeactivateTicket (int ticketId); // make it used

    }
}
