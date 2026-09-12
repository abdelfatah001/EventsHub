using Domain.Entities;
using System.Reflection;

namespace Domain.Interfaces
{
    public interface IEventOccurenceRepo
    {
        Task<EventOccurence?> AddAsync(EventOccurence eventOccurnce);
        Task<EventOccurence?> GetByIdAsync(int eventOccurenceId);
        Task<IEnumerable<EventOccurence>?> GetAllAsync();
        Task<IEnumerable<EventOccurence>?> GetAllByEventIdAsync(int eventId);
        Task<bool> UpdateAsync(EventOccurence eventOccurence);

        // to see is it booked in this date and time or not to be booked
        Task<bool> IsHallAvaliableAsync(int hallId, DateTime startDate, DateTime endDate);

        // to ensure is that hall booked in future or not to deactivate it
        Task<bool> IsHallBooked(int hallId);

    }
}
