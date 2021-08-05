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
        public bool IsAuthorised { get; }
        public UserDto CurrentUser { get;  }
    }
}
