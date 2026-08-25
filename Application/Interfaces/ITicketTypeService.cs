using Application.DTOs.TicketType.Request;
using Application.DTOs.TicketType.Respond;

namespace Application.Interfaces
{
    public interface ITicketTypeService
    {
        Task<TicketTypeDetailedResponseDto> AddAsync(AddTicketTypeDto ticketType);

        // for admin and organizer
        Task<IEnumerable<TicketTypeDetailedResponseDto>> GetTicketTypesByOccurenceIdAsync(int occurenceId);

        // for users
        Task<IEnumerable<TicketTypeResponseDto>> GetTicketTypesByOccurenceIdForUsersAsync(int occurenceId);

        Task<TicketTypeResponseDto> GetByIdAsync(int id);

        Task<bool> UpdateAsync (int id);

        Task DecrementAvaliableQuantityAsyncBy(int ticketTypeId, int quantityToDecrement);
        Task IncrementAvaliableQuantityAsyncBy(int ticketTypeId, int quantityToIncrement);


    }
}
