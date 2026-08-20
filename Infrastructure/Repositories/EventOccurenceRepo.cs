using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EventOccurenceRepo : IEventOccurenceRepo
    {
        private readonly AppDbContext _context;

        public EventOccurenceRepo(AppDbContext context)
        {
            _context = context;
        }


        public async Task<EventOccurence?> AddAsync(EventOccurence eventOccurence)
        {
            ArgumentNullException.ThrowIfNull(eventOccurence, nameof(eventOccurence));

            _context.EventOccurences.Add(eventOccurence);

            return await _context.SaveChangesAsync() > 0 ? eventOccurence : null;
        }

        public async Task<IEnumerable<EventOccurence>> GetAllAsync()
        {
            return await _context.EventOccurences
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<EventOccurence?> GetByIdAsync(int eventOccurenceId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(eventOccurenceId, nameof(eventOccurenceId));

            return await _context.EventOccurences
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == eventOccurenceId);
        }

        public async Task<bool> UpdateAsync(EventOccurence eventOccurence)
        {
            ArgumentNullException.ThrowIfNull(eventOccurence, nameof(eventOccurence));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(eventOccurence.Id, nameof(eventOccurence.Id));

            _context.EventOccurences.Update(eventOccurence);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
