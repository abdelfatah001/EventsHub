using AutoMapper;
using Domain.Entities;
using Application.Users.Dto.Request;
using Application.Users.Dto.Response;

namespace Application.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AddUserDto, Dto>();

            CreateMap<Dto, UserResponseDto>();

            CreateMap<UpdateEmailDto, Dto>();
            CreateMap<UpdatePasswordDto, Dto>();
            CreateMap<UpdateProfileDto, Dto>();

            CreateMap<Dto, UserDetailedResponseDto>();
        }
    }
}
