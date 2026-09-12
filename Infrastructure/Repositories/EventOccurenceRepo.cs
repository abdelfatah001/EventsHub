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

        public async Task<IEnumerable<EventOccurence>?> GetAllAsync()
        {
            return await _context.EventOccurences
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<EventOccurence>?> GetAllByEventIdAsync(int eventId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(eventId, nameof(eventId));

            return await _context.EventOccurences
                .AsNoTracking()
                .Where(o => o.EventId == eventId)
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

        public async Task<bool> IsHallAvaliableAsync (int hallId, DateTime startDate,  DateTime endDate)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(hallId);

            if (startDate >= endDate)
                throw new ArgumentException();

            return !await _context.EventOccurences
                .AsNoTracking()
                .AnyAsync(x =>
                    (x.HallId == hallId) && 
                    (x.StartDate < endDate) && 
                    (x.EndDate > startDate));
        }

        // to ensure is that hall booked in future or not to deactivate it
        public async Task<bool> IsHallBooked(int hallId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(hallId);

            var now = DateTime.UtcNow;

            return await _context.EventOccurences
                .AsNoTracking()
                .AnyAsync(h => h.HallId == hallId && h.StartDate > now);
        }

    }
}
