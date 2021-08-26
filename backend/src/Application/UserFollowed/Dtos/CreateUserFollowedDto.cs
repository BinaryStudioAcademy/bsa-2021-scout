using Application.Common.Models;
using Application.Users.Dtos;
using Domain.Enums;

namespace Application.UserFollowed.Dtos
{
    public class CreateUserFollowedDto
    {
        public string EntityId { get; set; }
        public EntityType EntityType { get; set; }
    }
}