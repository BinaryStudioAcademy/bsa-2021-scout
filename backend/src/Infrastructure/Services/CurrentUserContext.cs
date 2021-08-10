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
using Domain.Interfaces.Read;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class CurrentUserContext : ICurrentUserContext
    {
        public string Email { get; }
        public bool IsAuthorized { get; }
        public Task<User> CurrentUser { get; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserReadRepository _userRepository;

        public CurrentUserContext(IHttpContextAccessor httpContextAccessor, IUserReadRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;

            Email = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            IsAuthorized = Email != null;
            CurrentUser = _userRepository.GetByEmailAsync(Email);
        }
    }
}
