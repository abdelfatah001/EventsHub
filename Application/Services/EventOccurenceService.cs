using Application.DTOs.EventOccurences.Request;
using Application.DTOs.EventOccurences.Response;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services
{
    public class EventOccurenceService : IEventOccurenceService
    {
        private readonly IEventOccurenceRepo _repo;

        public EventOccurenceService(IEventOccurenceRepo repo)
        {
            _repo = repo;
        }

        public Task<OccurenceResponseDto?> AddAsync(AddOccurenceDto eventCategory)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OccurenceResponseDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OccurenceResponseDto?> GetByIdAsync(int eventCategoryId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(UpdateOccurenceDto eventCategory)
        {
            throw new NotImplementedException();
        }
    }
}
