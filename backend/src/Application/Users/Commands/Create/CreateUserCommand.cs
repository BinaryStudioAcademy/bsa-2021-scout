using Application.Users.Dtos;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Entities;
using AutoMapper;

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
        private readonly IWriteRepository<User> _writeRepository;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IWriteRepository<User> writeRepository, IMapper mapper)
        {
            _writeRepository = writeRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken _)
        {
            var user = _mapper.Map<User>(request.User);
            return _mapper.Map<UserDto>(await _writeRepository.CreateAsync(user));
        }
    }
}
