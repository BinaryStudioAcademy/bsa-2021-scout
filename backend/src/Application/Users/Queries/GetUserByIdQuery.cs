using Application.Users.Dtos;
using Application.Common.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;
using Domain.Interfaces.Read;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public string Id { get; }

        public GetUserByIdQuery(string id)
        {
            Id = id;
        }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        protected readonly IUserReadRepository _repository;

        protected readonly IMapper _mapper;
        protected readonly ISender _mediator;

        public GetUserByIdQueryHandler(IUserReadRepository repository, IMapper mapper,
         ISender mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<UserDto> Handle(GetUserByIdQuery query, CancellationToken _)
        {
            var user = await _repository.GetByIdAsync(query.Id);
            return _mapper.Map<UserDto>(user);
        }
    }
}
