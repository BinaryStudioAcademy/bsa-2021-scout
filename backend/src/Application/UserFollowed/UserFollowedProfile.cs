using Application.UserFollowed.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.UserFollowed
{
   public class UserFollowedProfile : Profile
    {
        public UserFollowedProfile()
        {
            CreateMap<CreateUserFollowedDto, UserFollowedDto>();
            CreateMap<CreateUserFollowedDto, UserFollowedEntity>();
            CreateMap<UserFollowedEntity, UserFollowedDto>();
            CreateMap<UserFollowedDto, UserFollowedEntity>();

        }
    }
}