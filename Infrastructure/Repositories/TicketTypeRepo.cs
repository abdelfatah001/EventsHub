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

        public async Task<bool> UpdateAsync(TicketType ticketType)
        {
            ArgumentNullException.ThrowIfNull(ticketType);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(ticketType.Id);

            _context.TicketTypes.Update(ticketType);

            return await _context.SaveChangesAsync() > 0;

        }
    }
}
