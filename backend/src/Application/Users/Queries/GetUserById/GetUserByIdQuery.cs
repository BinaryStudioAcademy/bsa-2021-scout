using Application.Interfaces;
using Application.Users.Dtos;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public GetUserByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IService<UserDto> _service;
        public GetUserByIdQueryHandler(IService<UserDto> service)
        {
            _service = service;
        }

        public Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken _)
        {
            return _service.GetAsync(request.Id);
        }
    }
}
