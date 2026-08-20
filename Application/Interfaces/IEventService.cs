
using Application.DTOs.Events.Request;
using Application.DTOs.Events.Response;

namespace Application.Interfaces
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
