using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class EventCategoryRepo : IEventCategoryRepo
    {

        private readonly AppDbContext _context;

        public EventCategoryRepo (AppDbContext context)
        {
            _context = context;
        }

        public async Task<EventCategory?> AddAsync(EventCategory eventCategory)
        {
            ArgumentNullException.ThrowIfNull(eventCategory, nameof(eventCategory));

            _context.EventCategories.Add(eventCategory);

            return await _context.SaveChangesAsync() > 0 ? eventCategory : null;

        }

        public async Task<IEnumerable<EventCategory>> GetAllAsync()
        {
            return await _context.EventCategories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<EventCategory?> GetByIdAsync(int eventCategoryId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(eventCategoryId, nameof(eventCategoryId));

            return await _context.EventCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == eventCategoryId);
        }

        public async Task<bool> UpdateAsync(EventCategory eventCategory)
        {
            ArgumentNullException.ThrowIfNull(eventCategory, nameof(eventCategory));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(eventCategory.Id, nameof(eventCategory.Id));

            _context.EventCategories.Update(eventCategory);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}