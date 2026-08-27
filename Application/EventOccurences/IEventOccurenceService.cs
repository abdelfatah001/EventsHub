using Application.EventOccurences.Dto.Request;
using Application.EventOccurences.Dto.Response;
using Domain.Entities;

namespace Application.EventOccurences
{
    public interface IEventOccurenceService
    {
        Task<OccurenceResponseDto?> AddAsync(AddOccurenceDto eventOccurence);
        Task<OccurenceResponseDto?> GetByIdAsync(int eventOccurenceId);
        Task<IEnumerable<OccurenceResponseDto>> GetAllAsync();
        Task<IEnumerable<OccurenceResponseDto>> GetAllByEventIdAsync(int eventId);
        Task<bool> UpdateAsync(int Id, UpdateOccurenceDto eventOccurence);
    }
}
