using Application.DTOs.EventOccurences.Request;
using Application.DTOs.EventOccurences.Response;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IEventOccurenceService
    {
        Task<OccurenceResponseDto?> AddAsync(AddOccurenceDto eventCategory);
        Task<OccurenceResponseDto?> GetByIdAsync(int eventCategoryId);
        Task<IEnumerable<OccurenceResponseDto>> GetAllAsync();
        Task<bool> UpdateAsync(UpdateOccurenceDto eventCategory);
    }
}
