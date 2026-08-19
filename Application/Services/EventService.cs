using Application.DTOs.Events.Request;
using Application.DTOs.Events.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepo _eventRepo;

        private readonly IMapper _mapper;
        public EventService (IEventRepo eventRepo, IMapper mapper)
        {
            _eventRepo = eventRepo;
            _mapper = mapper;
        }
        public async Task<EventResponseDto?> AddAsync(AddEventDto @event)
        {
            if (@event is null)
                throw new ArgumentNullException(nameof(@event), "New event payload cannot be null.");

            var _event = await _eventRepo.AddAsync(_mapper.Map<Event>(@event));

            return _mapper.Map<EventResponseDto?>(_event);

        }

        public async Task<IEnumerable<EventResponseDto>> GetAllAsync()
        {
            var events = await _eventRepo.GetAllAsync();

            return _mapper.Map<IEnumerable<EventResponseDto>>(events);
        }

        public async Task<IEnumerable<EventResponseDto>> GetAllByCategoryIdAsync(int categoryId)
        {
            if (categoryId <= 0)
                throw new ArgumentOutOfRangeException(nameof(categoryId), categoryId, "Category id must be a positive integer.");

            var events = await _eventRepo.GetAllByCategoryIdAsync(categoryId);

            return _mapper.Map<IEnumerable<EventResponseDto>>(events);
        }

        public async Task<IEnumerable<EventResponseDto>> GetAllByOrganizerIdAsync(int organizerId)
        {
            if (organizerId <= 0)
                throw new ArgumentOutOfRangeException(nameof(organizerId), organizerId, "Organizer id must be a positive integer.");

            var events = await _eventRepo.GetAllByOrganizerIdAsync(organizerId);

            return _mapper.Map<IEnumerable<EventResponseDto>>(events);
        }

        public async Task<EventResponseDto?> GetByIdAsync(int eventId)
        {
            if (eventId <= 0)
                throw new ArgumentOutOfRangeException(nameof(eventId), eventId, "Event id must be a positive integer.");

            var events = await _eventRepo.GetByIdAsync(eventId);

            return _mapper.Map<EventResponseDto>(events);
        }

        public async Task<bool> UpdateAsync(int Id, UpdateEventDto @event)
        {
            if (Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(Id), Id, "Event id must be a positive integer.");

            var _event = await _eventRepo.GetByIdAsync(Id);

            if (_event is null)
                throw new ArgumentNullException(nameof(_event), $"User with ID {Id} does not exist.");
            
            @event.Title = @event.Title;
            @event.Description = @event.Description;
            @event.CategoryId = @event.CategoryId;

            return await _eventRepo.UpdateAsync(_mapper.Map<Event>(@event));
        }
        public async Task<bool> DeleteAsync(int eventId)
        {
            if (eventId <= 0)
                throw new ArgumentOutOfRangeException(nameof(eventId), eventId, "Event id must be a positive integer.");

            var _event = await _eventRepo.GetByIdAsync(eventId);

            if (_event is null)
                throw new ArgumentNullException(nameof(_event), $"User with ID {eventId} does not exist.");

            return await _eventRepo.DeleteAsync(_event);

        }

    }
}
