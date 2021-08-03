using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Auth.Dtos;
using Application.Interfaces;
using Application.Users.Dtos;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Auth.Commands
{
    public class IsAuthorisedCommand : IRequest<UserDto>
    {
    }

    public class IsAuthorisedCommandHandler : IRequestHandler<IsAuthorisedCommand, UserDto>
    {
        protected readonly ISender _mediator;
        protected readonly IUserReadRepository _userRepository;

        protected readonly ICurrentUserContext _currentUserContext;
        protected readonly IMapper _mapper;

        public IsAuthorisedCommandHandler(ISender mediator, IUserReadRepository userRepository,
                                   ICurrentUserContext currentUserContext, IMapper mapper)
        {
            _mediator = mediator;
            _userRepository = userRepository;
            _currentUserContext = currentUserContext;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(IsAuthorisedCommand command, CancellationToken _)
        {
            if (_currentUserContext.IsAuthorised)
                return await _currentUserContext.LoadUser();
            else
                throw new Exception("The user is anonymous");
        }
    }
}
