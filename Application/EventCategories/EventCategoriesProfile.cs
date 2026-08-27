using Application.EventOccurences.Dto.Request;
using Application.EventOccurences.Dto.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.EventCategories
{
    public class EventCategoriesProfile : Profile
    {
        public EventCategoriesProfile() 
        {
            CreateMap<CategoryRequestDto, EventCategory>();

            CreateMap<EventCategory, CategoryResponseDto>();
        }
    }
}
