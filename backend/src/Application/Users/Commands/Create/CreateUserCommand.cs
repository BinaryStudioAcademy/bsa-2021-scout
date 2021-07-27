using Application.Users.Dtos;
using Application.Generics.Commands;
using Domain.Interfaces;
using Domain.Entities;
using AutoMapper;

namespace Application.Users.Commands.Create
{
    public class CreateUserCommand : CreateEntityCommand<UserDto>
    {
        public CreateUserCommand(UserDto user) : base(user) { }
    }

    public class CreateUserCommandHandler : CreateEntityCommandHandler<User, UserDto>
    {
        public CreateUserCommandHandler(IWriteRepository<User> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
