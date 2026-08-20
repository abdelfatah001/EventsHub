using Domain.Entities;
using System.Reflection;

namespace Domain.Interfaces
{
    public interface IEventOccurenceRepo
    {
        Task<EventOccurence?> AddAsync(EventOccurence eventOccurnce);
        Task<EventOccurence?> GetByIdAsync(int eventOccurenceId);
        Task<IEnumerable<EventOccurence>> GetAllAsync();
        Task<bool> UpdateAsync(EventOccurence eventOccurence);
    }
}
