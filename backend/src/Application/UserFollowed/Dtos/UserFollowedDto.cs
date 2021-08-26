using Application.Common.Models;
using Application.Users.Dtos;
using Domain.Enums;

namespace Application.UserFollowed.Dtos
{
    public class UserFollowedDto: Dto
    {
        public string UserId { get; set; }
        public UserDto User { get; set; }
        public string EntityId { get; set; }
        public EntityType EntityType { get; set; }
    }
}