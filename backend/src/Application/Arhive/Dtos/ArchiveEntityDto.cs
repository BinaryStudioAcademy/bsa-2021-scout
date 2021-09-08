using Application.Common.Models;
using Application.Users.Dtos;
using Domain.Enums;
using System;

namespace Application.Arhive.Dtos
{
    public class ArchivedEntityDto : Dto
    {
        public string EntityId { get; set; }
        public EntityType EntityType { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string UserId { get; set; }
        public UserDto User { get; set; }

    }
}
