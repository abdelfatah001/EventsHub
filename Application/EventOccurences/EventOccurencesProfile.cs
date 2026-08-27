using Application.EventOccurences.Dto.Request;
using Application.EventOccurences.Dto.Response;
using Application.EventOccurences.Dto.Request;
using Application.EventOccurences.Dto.Response;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.EventOccurences
{
    public class EventOccurencesProfile : Profile
    {
        public EventOccurencesProfile()
        {
            CreateMap<AddOccurenceDto, EventOccurence>();
            CreateMap<UpdateOccurenceDto, EventOccurence>();


            CreateMap<EventOccurence, OccurenceResponseDto>();
        }

    }
}
