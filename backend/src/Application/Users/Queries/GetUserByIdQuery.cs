using Application.Users.Dtos;
using Application.Common.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;

namespace Application.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : GetEntitiesQueryHandler<User, UserDto>
    {
        public GetUserByIdQueryHandler(IReadRepository<User> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
