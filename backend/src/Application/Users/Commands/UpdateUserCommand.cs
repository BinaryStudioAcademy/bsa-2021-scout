using Application.Users.Dtos;
using Application.Common.Commands;
using Domain.Interfaces.Abstractions;
using Domain.Entities;
using AutoMapper;

namespace Application.Users.Commands.Create
{
    public class UpdateUserCommandHandler : UpdateEntityCommandHandler<User, UserDto>
    {
        public UpdateUserCommandHandler(IWriteRepository<User> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
