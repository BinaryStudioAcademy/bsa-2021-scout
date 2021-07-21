using Application.Users.Dtos;
using Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.Create
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public CreateUserCommand(UserDto user)
        {
            User = user;
        }

        public UserDto User { get; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IService<UserDto> _service;

        public CreateUserCommandHandler(IService<UserDto> service)
        {
            _service = service;
        }

        public Task<UserDto> Handle(CreateUserCommand request, CancellationToken _)
        {
            return _service.CreateAsync(request.User);
        }
    }
}
