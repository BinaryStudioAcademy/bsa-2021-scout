using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Users.Dtos;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class CurrentUserContext : ICurrentUserContext
    {
        
        public bool IsAuthorised { get; set; }
        public UserDto CurrentUser { get; set; }
        public RoleDto[] Roles { get; set; }
        
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            
            
            var user = new UserDto();
            var claims = _httpContextAccessor.HttpContext?.User ;
            user.Email= _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email).Value;
            user.FirstName = "aaa";
            user.LastName = "BBB";
            user.MiddleName = "CCC";
            user.Roles = new RoleDto[0];
            user.BirthDate = DateTime.Now;
            user.Id = "1234";
            CurrentUser = user;
            //var role = httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value == roleType;


            IsAuthorised = CurrentUser.Id != null;
        }
        //public async Task<UserDto> IsAuth()
        //{
            
        //    return user;
        //}
    }
}
