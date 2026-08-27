using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITicketTypeRepo
    {
        Task<TicketType?> AddAsync (TicketType ticketType);

        // get this occurence TicketTypes 
        Task<IEnumerable<TicketType>> GetAllByOccurenceIdAsync (int occurenceId);

        Task<TicketType?> GetByIdAsync(int ticketTypeId);
        Task<bool> UpdateAsync(TicketType ticketType);

        Task<bool> IsThereAvaliableTicketsAsync (int ticketTypeId);

        Task DecrementAvaliableQuantityAsyncBy(int ticketTypeId, int quantityToDecrement);
        Task IncrementAvaliableQuantityAsyncBy(int ticketTypeId, int quantityToIncrement);

    }
}
