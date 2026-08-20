using Application.DTOs.Category.Request;
using Application.DTOs.Category.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
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
