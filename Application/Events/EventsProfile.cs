using Application.Events.Dto.Request;
using Application.Events.Dto.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.Events
{
    public class EventsProfile : Profile
    {

        public EventsProfile ()
        {
            CreateMap<AddEventDto, Event>();
            CreateMap<UpdateEventDto, Event>();

            CreateMap<Event, EventResponseDto>();
        }
    }
}
