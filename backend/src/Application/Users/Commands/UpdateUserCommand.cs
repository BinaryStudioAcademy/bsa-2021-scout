using Application.Users.Dtos;
using Application.Common.Commands;
using Domain.Interfaces.Abstractions;
using Domain.Entities;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Users.Queries;
using System.Collections.Generic;
using System.Linq;

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
        protected readonly ICurrentUserContext _currentUser;

        protected readonly IMapper _mapper;
        private readonly ISender _mediator;

        public UpdateProjectCommandHandler(IWriteRepository<User> writeRepository,
            IReadRepository<User> readRepository, IMapper mapper, ISender mediator, ICurrentUserContext currentUser)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
            _mapper = mapper;
            _mediator = mediator;
            _currentUser = currentUser;
        }

        public async Task<UserDto> Handle(UpdateUserCommand command, CancellationToken _)
        {
            UserDto currentUser = await _currentUser.GetCurrentUser();

            List<UserDto> selectedUsers = new List<UserDto>();

            if (currentUser.Roles.Where(x => x.Name == "HrLead").ToList().Count !=0)
            {
                var query = new GetUsersForHrLeadQuery();
                selectedUsers = (await _mediator.Send(query)).ToList();
            }
            
            if (currentUser.Id == command.User.Id || selectedUsers.Where(x=>x.Id == command.User.Id).ToList().Count != 0)
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

            throw new System.Exception("The user was not found for editing");

        }
    }
}
