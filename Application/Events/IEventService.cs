using Application.Events.Dto.Request;
using Application.Events.Dto.Response;

namespace Application.Events
{
    public interface IEventService
    {
        Task<EventResponseDto?> AddAsync(AddEventDto @event);
        Task<EventResponseDto?> GetByIdAsync(int eventId);
        Task<IEnumerable<EventResponseDto>> GetAllAsync();
        Task<IEnumerable<EventResponseDto>> GetAllByCategoryIdAsync(int categoryId);
        Task<IEnumerable<EventResponseDto>> GetAllByOrganizerIdAsync(int organizerId);
        Task<bool> UpdateAsync(int Id, UpdateEventDto @event);
        Task<bool> DeleteAsync(int eventId);
        Task<bool> ExistsAsync(int eventId);

    }
}
