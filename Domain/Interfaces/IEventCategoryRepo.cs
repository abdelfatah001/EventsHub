using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IEventCategoryRepo
    {
        Task<EventCategory?> AddAsync(EventCategory eventCategory);
        Task<EventCategory?> GetByIdAsync(int eventCategoryId);
        Task<IEnumerable<EventCategory>> GetAllAsync();
        Task<bool> UpdateAsync(EventCategory eventCategory);
    }
}
