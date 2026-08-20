using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IEventRepo
    {
        Task<Event?> AddAsync(Event @event);
        Task<Event?> GetByIdAsync(int eventId);
        Task<IEnumerable<Event>> GetAllAsync();
        Task<IEnumerable<Event>> GetAllByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Event>> GetAllByOrganizerIdAsync(int organizerId);
        Task<bool> UpdateAsync(Event @event);
        Task<bool> DeleteAsync(Event @event);
        Task<bool> ExistsAsync (int eventId);
    }
}
