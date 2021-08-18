using Application.Common.Queries;
using Application.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;

namespace Application.Users.Queries.GetUserById
{
    public class GetUserByEmailQueryHandler : GetEntityByPropertyQueryHandler<User, UserDto>
    {
        public GetUserByEmailQueryHandler(IReadRepository<User> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
