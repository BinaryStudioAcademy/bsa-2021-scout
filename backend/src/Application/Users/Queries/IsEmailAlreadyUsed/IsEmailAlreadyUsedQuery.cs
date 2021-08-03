using Application.Users.Dtos;
using Application.Common.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;

namespace Application.Users.Queries.GetUserById
{
    public class IsEmailAlreadyUsedQueryHandler : IsEntityWithPropertyExistQueryHandler<User>
    {
        public IsEmailAlreadyUsedQueryHandler(IReadRepository<User> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
