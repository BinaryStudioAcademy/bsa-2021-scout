using Application.Auth.Commands;
using Application.Auth.Dtos;
using Application.Interfaces;
using Application.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public class RegisterUserCommand : IRequest<AuthUserDto>
    {
        public UserRegisterDto RegisterUser { get; }

        public RegisterUserCommand(UserRegisterDto registerUser)
        {
            RegisterUser = registerUser;
        }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthUserDto>
    {
        protected readonly ISender _mediator;
        protected readonly IWriteRepository<User> _userRepository;

        protected readonly ISecurityService _securityService;
        protected readonly IMapper _mapper;

        public RegisterUserCommandHandler(ISender mediator, IWriteRepository<User> userRepository,
                                   ISecurityService securityService, IMapper mapper)
        {
            _mediator = mediator;
            _userRepository = userRepository;
            _securityService = securityService;
            _mapper = mapper;
        }

        public async Task<AuthUserDto> Handle(RegisterUserCommand command, CancellationToken _)
        {
            var newUser = _mapper.Map<User>(command.RegisterUser);
            var salt = _securityService.GetRandomBytes();

            newUser.PasswordSalt = Convert.ToBase64String(salt);
            newUser.Password = _securityService.HashPassword(command.RegisterUser.Password, salt);

            var registeredUser = await _userRepository.CreateAsync(newUser) as User;

            var generateTokenCommand = new GenerateAccessTokenCommand(registeredUser);
            var token = await _mediator.Send(generateTokenCommand);

            return new AuthUserDto
            {
                User = _mapper.Map<UserDto>(registeredUser),
                Token = token
            };
        }
    }
}
