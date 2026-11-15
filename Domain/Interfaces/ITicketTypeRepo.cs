using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITicketTypeRepo
    {
        Task<TicketType?> AddAsync (TicketType ticketType);

        // get this occurence TicketTypes 
        Task<IEnumerable<TicketType>> GetAllByOccurenceIdAsync (int occurenceId);

        Task<TicketType?> GetByIdAsync(int ticketTypeId);
        Task<decimal> GetPriceAsync(int ticketTypeId);

        Task<bool> UpdateAsync(TicketType ticketType);

        Task<bool> IsThereAvaliableTicketsAsync(int ticketTypeId, int Quantity);

        // check is there are enough tickets available and reserve them if possible, return the price of unit of the ticket reserved
        Task<decimal?> TryReserveAsync(int ticketTypeId, int quantity);



    }
}
