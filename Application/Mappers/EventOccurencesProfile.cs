using Application.DTOs.Category.Request;
using Application.DTOs.Category.Response;
using Application.DTOs.EventOccurences.Request;
using Application.DTOs.EventOccurences.Response;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Mappers
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
