using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITicketRepo
    {
        Task<Ticket?> AddAsync(Ticket ticket);

        Task<IEnumerable<Ticket>> GetAllByTicketTypeIdAsync(int ticketTypeId);

        Task<IEnumerable<Ticket>> GetAllByOrderItemIdAsync(int orderItemId);

        Task<Ticket?> GetByTicketCodeAsync(string ticketCode);

        Task<Ticket?> GetByIdAsync (int ticketId);

        Task<bool> UpdateAsync(Ticket ticket);

        Task<bool> IsCodeUniqueAsync(string code);

    }
}
