using AutoMapper;
using Application.DTOs.User.Request;
using Domain.Entities;
using Application.DTOs.User.Response;

namespace Application.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AddUserDto, User>();

            CreateMap<User, UserResponseDto>();

            CreateMap<UpdateEmailDto, User>();
            CreateMap<UpdatePasswordDto, User>();
            CreateMap<UpdateProfileDto, User>();

            CreateMap<User, UserDetailedResponseDto>();
        }
    }
}
