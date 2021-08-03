using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Users.Dtos;

namespace Application.Interfaces
{
    public interface ICurrentUserContext
    {
        bool IsAuthorised { get; set; }
        UserDto CurrentUser { get; set; }
        RoleDto[] Roles { get; set; }
    }
}
