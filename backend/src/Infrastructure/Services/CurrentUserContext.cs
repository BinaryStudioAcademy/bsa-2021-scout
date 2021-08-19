using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Auth.Exceptions;
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

        private UserDto currentUser { get; set; }

        private readonly IUserReadRepository _userRepository;
        private readonly IMapper _mapper;

        public CurrentUserContext(IHttpContextAccessor httpContextAccessor, IUserReadRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;

            Email = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            IsAuthorized = Email != null;
        }

        public async Task<UserDto> GetCurrentUser()
        {

            if (IsAuthorized)
            {
                if(currentUser is null)
                    currentUser = _mapper.Map<UserDto>(await _userRepository.GetByEmailAsync(Email));
            }
            else{
                throw new InvalidTokenException("access");
            }

            return currentUser;
        }
    }
}
