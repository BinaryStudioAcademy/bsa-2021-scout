using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.EF;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class CurrentUserContext : ICurrentUserContext
    {
        
        public bool IsAuthorised { get; }
        public UserDto CurrentUser { get; private set; }
        public RoleDto[] Roles { get; private set; }
        public Lazy<Task<User>> LazyUser { get; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserReadRepository _userRepository;
        private readonly IMapper _mapper;

        public CurrentUserContext(IHttpContextAccessor httpContextAccessor, IUserReadRepository userRepository, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _mapper = mapper;

            var Email = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            IsAuthorised = Email != null;
            LazyUser = new Lazy<Task<User>>(_userRepository.GetByEmailAsync(Email));
        }
        public async Task<UserDto> LoadUser()
        {
            User user = await LazyUser;
            CurrentUser = _mapper.Map<UserDto>(user);
            Roles = CurrentUser.Roles.ToArray();
            return CurrentUser;
        }
    }
        static class Program
        {
            public static TaskAwaiter<T> GetAwaiter<T>(this Lazy<Task<T>> asyncTask)
            {
                return asyncTask.Value.GetAwaiter();
            }
        }
        
    
}
