using Application.DTOs.EventOccurences.Request;
using Application.DTOs.EventOccurences.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class EventOccurenceService : IEventOccurenceService
    {
        private readonly IEventOccurenceRepo _Occurencerepo; 
        private readonly IEventRepo _eventRepo;
        private readonly IMapper _mapper;
        public EventOccurenceService(IEventOccurenceRepo occurenceRepo, IEventRepo eventRepo, IMapper mapper)
        {
            _Occurencerepo = occurenceRepo;
            _eventRepo = eventRepo;
            _mapper = mapper;
        }

        public async Task<OccurenceResponseDto?> AddAsync(AddOccurenceDto newOccurence)
        {
            if (newOccurence is null)
                throw new ArgumentNullException(nameof(newOccurence), "New event occurence payload cannot se null");

            var eventExists = await _eventRepo.ExistsAsync(newOccurence.EventId);

            // Is this event is existed 
            if (!eventExists)
                throw new KeyNotFoundException($"Event with ID {newOccurence.EventId} was not found.");

            // check is there overlap in time or location
            
            var occurence = await _Occurencerepo.AddAsync(_mapper.Map<EventOccurence>(newOccurence));

            return _mapper.Map<OccurenceResponseDto>(occurence);
        }

        public async Task<IEnumerable<OccurenceResponseDto>> GetAllAsync()
        {
            
        }

        public async Task<OccurenceResponseDto?> GetByIdAsync(int eventCategoryId)
        {
          
        }

        public async Task<bool> UpdateAsync(UpdateOccurenceDto eventCategory)
        {
           
        }

    }
}
