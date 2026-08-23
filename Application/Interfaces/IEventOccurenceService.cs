using Application.DTOs.EventOccurences.Request;
using Application.DTOs.EventOccurences.Response;
using Domain.Entities;

namespace Application.Interfaces
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
