using Application.TicketTypes.Dto.Request;
using Application.TicketTypes.Dto.Respond;

namespace Application.TicketTypes
{
    public interface ITicketTypeService
    {
        Task<TicketTypeDetailedResponseDto> AddAsync(AddTicketTypeDto ticketType);

        // for admin and organizer
        Task<IEnumerable<TicketTypeDetailedResponseDto>> GetTicketTypesByOccurenceIdAsync(int occurenceId);

        // for users
        Task<IEnumerable<TicketTypeResponseDto>> GetTicketTypesByOccurenceIdForUsersAsync(int occurenceId);

        Task<TicketTypeResponseDto> GetByIdAsync(int id);

        Task<bool> UpdateAsync (int id, UpdateTicketTypeDto updateTicketType);


    }
}
