using Application.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using System.Linq;

namespace Application.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Role, RoleDto>().ReverseMap();

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(p => p.Role)))
                .ForMember(dest=>dest.AvatarUrl, opt=>opt.MapFrom(src=>src.Avatar.PublicUrl));

            CreateMap<UserDto, User>()
                .ForMember(dest => dest.DomainEvents, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src =>
                    src.Roles.Select(role => new UserToRole()
                    {
                        RoleId = role.Id
                    })));

            CreateMap<UserUpdateDto, User>()
                .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());

            CreateMap<UserRegisterDto, User>()
                .ForMember(dest => dest.DomainEvents, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src =>
                    src.Roles.Select(role => new UserToRole()
                    {
                        RoleId = role.Id
                    })));

            CreateMap<RegisterPermission, RegisterPermissionShortDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Expires));
        }
    }
}
