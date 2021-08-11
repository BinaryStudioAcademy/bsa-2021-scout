using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Users.Dtos;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICurrentUserContext
    {
        public string Email { get; }
        public bool IsAuthorized { get; }
        public Task<UserDto> GetCurrentUser();
    }
}
