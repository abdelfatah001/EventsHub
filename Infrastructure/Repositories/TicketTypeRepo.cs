using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TicketTypeRepo : ITicketTypeRepo
    {
        private readonly AppDbContext _context;

        public TicketTypeRepo(AppDbContext context)
        {
            _context = context;
        }
       
        public async Task<TicketType?> AddAsync(TicketType ticketType)
        {
            ArgumentNullException.ThrowIfNull(ticketType);

            _context.TicketTypes.Add(ticketType);

            return await _context.SaveChangesAsync() > 0 ? ticketType : null;
        }

        public async Task<IEnumerable<TicketType>> GetAllByOccurenceIdAsync(int occurenceId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(occurenceId);

            return await _context.TicketTypes
                .AsNoTracking()
                .Where(x => x.OccurenceId == occurenceId)
                .ToListAsync();
        }

        public async Task<TicketType?> GetByIdAsync(int ticketTypeId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(ticketTypeId);

            return await _context.TicketTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == ticketTypeId);

        }

        public async Task<bool> IsThereAvaliableTicketsAsync(int ticketTypeId, int Quantity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(ticketTypeId);

            int avaliableTickets = await _context.TicketTypes
                .AsNoTracking()
                .Where(x => x.Id == ticketTypeId)
                .Select(x => x.AvailableQuantity)
                .FirstOrDefaultAsync();

            return avaliableTickets >= Quantity;
        }

        public async Task<bool> UpdateAsync(TicketType ticketType)
        {
            ArgumentNullException.ThrowIfNull(ticketType);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(ticketType.Id);

            _context.TicketTypes.Update(ticketType);

            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<bool> TryReserveAsync(int ticketTypeId, int quantity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(ticketTypeId);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

            var affectedRows = await _context.TicketTypes
                .Where(t => t.Id == ticketTypeId && t.AvailableQuantity >= quantity)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(
                        t => t.AvailableQuantity,
                        t => t.AvailableQuantity - quantity));

            return affectedRows > 0;

        }

        public async Task<double> GetPriceAsync(int ticketTypeId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(ticketTypeId);

            return await _context.TicketTypes
                .Where(x => x.Id == ticketTypeId)
                .Select(x => x.Price)
                .FirstOrDefaultAsync();

        }
    }
}
