using Application.Auth.Dtos;
using Application.Common.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;

namespace Application.Users.Queries.GetUserById
{
    public class GetEmailTokenByUserIdQueryHandler : GetEntityByPropertyQueryHandler<EmailToken, EmailTokenDto>
    {
        public GetEmailTokenByUserIdQueryHandler(IReadRepository<EmailToken> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
