using Application.Tickets.Dto.Response;

namespace Application.Tickets
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketResponseDto>> AddTicketsAsync(int ticketTypeId, int orderItemId, int Amount);

        Task<IEnumerable<TicketDetailedResponseDto>> GetAllByTicketTypeIdAsync(int ticketTypeId);

        Task<IEnumerable<TicketDetailedResponseDto>> GetAllByOrderItemIdAsync(int orderItemId);

        Task<TicketDetailedResponseDto> GetByIdAsync(int ticketId);

        Task<TicketDetailedResponseDto?> GetByTicketCodeAsync(string ticketCode);

        Task<bool> DeactivateTicket (int ticketId); // make it used

    }
}
