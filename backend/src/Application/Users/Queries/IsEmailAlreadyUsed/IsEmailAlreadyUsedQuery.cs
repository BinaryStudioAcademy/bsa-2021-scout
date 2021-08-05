using Application.Common.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;

namespace Application.Users.Queries.GetUserById
{
    public class IsEmailAlreadyUsedQueryHandler : IsEntityWithPropertyExistQueryHandler<User>
    {
        public IsEmailAlreadyUsedQueryHandler(IReadRepository<User> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
