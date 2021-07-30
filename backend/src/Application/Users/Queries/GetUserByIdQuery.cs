using Application.Users.Dtos;
using Application.Common.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using System;

namespace Application.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : GetEntityByIdQueryHandler<User, UserDto>
    {
        public GetUserByIdQueryHandler(IReadRepository<User> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
