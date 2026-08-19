using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DbContexts;

namespace Infrastructure.Repositories
{
    public class EventOccurenceRepo : IEventCategoryRepo
    {
        private readonly AppDbContext _context;

        public EventOccurenceRepo(AppDbContext context)
        {
            _context = context;
        }


        public Task<EventCategory?> AddAsync(EventCategory eventCategory)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EventCategory>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<EventCategory?> GetByIdAsync(int eventCategoryId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(EventCategory eventCategory)
        {
            throw new NotImplementedException();
        }
    }
}
