using System;
using System.Collections.Generic;

namespace Application.Users.Dtos
{
    public class UserRegisterDto
    {
        public UserRegisterDto()
        {
            Roles = new List<RoleDto>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        
        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<RoleDto> Roles { get; set; }
    }
}
