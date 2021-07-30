using Application.Users.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>()
                .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());
        }
    }
}
