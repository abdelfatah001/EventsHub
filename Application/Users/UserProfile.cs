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
            CreateMap<AddUserDto, User>();

            CreateMap<User, UserResponseDto>();

            CreateMap<UpdateEmailDto, User>();
            CreateMap<UpdatePasswordDto, User>();
            CreateMap<UpdateProfileDto, User>();

            CreateMap<User, UserDetailedResponseDto>();
        }
    }
}
