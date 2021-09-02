using Application.Users.Dtos;
using Application.Common.Commands;
using Domain.Interfaces.Abstractions;
using Domain.Entities;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.Create
{
    public class UpdateUserCommand : IRequest<UserDto>
    {
        public UserUpdateDto User { get; }

        public UpdateUserCommand(UserUpdateDto user)
        {
            User = user;
        }
    }

    public class UpdateProjectCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        protected readonly IWriteRepository<User> _writeRepository;
        protected readonly IReadRepository<User> _readRepository;
        protected readonly IMapper _mapper;
        private readonly ISender _mediator;

        public UpdateProjectCommandHandler(IWriteRepository<User> writeRepository,
            IReadRepository<User> readRepository, IMapper mapper, ISender mediator)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<UserDto> Handle(UpdateUserCommand command, CancellationToken _)
        {
            User userToUpdate = await _readRepository.GetAsync(command.User.Id);

            User entity = _mapper.Map<User>(command.User);

            userToUpdate.BirthDate = entity.BirthDate;
            userToUpdate.FirstName = entity.FirstName;
            userToUpdate.LastName = entity.LastName;
            userToUpdate.Phone = entity.Phone;
            userToUpdate.Skype = entity.Skype;


            var updated = await _writeRepository.UpdateAsync(userToUpdate);

            return _mapper.Map<UserDto>(updated);
        }
    }
}
