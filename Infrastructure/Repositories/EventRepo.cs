using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class EventRepo : IEventRepo
    {
        private readonly AppDbContext _context;

        public EventRepo(AppDbContext context)
        {   
            _context = context;
        }

        public async Task<Event?> AddAsync(Event @event)
        {
            ArgumentNullException.ThrowIfNull(@event, nameof(@event));

            _context.Events.Add(@event);

            return await _context.SaveChangesAsync() > 0 ? @event : null;
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _context.Events
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Event?> GetByIdAsync(int eventId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(eventId, nameof(eventId));

            return await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == eventId);
        }

        public async Task<IEnumerable<Event>> GetAllByCategoryIdAsync(int categoryId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(categoryId, nameof(categoryId));

            return await _context.Events
                .AsNoTracking()
                .Where(e => e.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetAllByOrganizerIdAsync(int organizerId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(organizerId, nameof(organizerId));

            return await _context.Events
                .AsNoTracking()
                .Where(e => e.OrganizerId == organizerId)
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(Event @event)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(@event.Id, nameof(@event.Id));

            _context.Events.Remove(@event);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Event @event)
        {
            ArgumentNullException.ThrowIfNull(@event, nameof(@event));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(@event.Id, nameof(@event.Id));

            _context.Events.Update(@event);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync (int eventId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(eventId, nameof(eventId));

            return await _context.Events
                .AnyAsync(e => e.Id == eventId);

        }

    }
}
