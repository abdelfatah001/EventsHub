using Application.DTOs.Events.Request;
using Application.DTOs.Events.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
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
