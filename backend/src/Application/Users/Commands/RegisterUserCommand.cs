using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Auth.Commands;
using Application.Auth.Dtos;
using Application.Interfaces;
using Application.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;

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
        protected readonly IWriteRepository<User> _userWriteRepository;

        protected readonly ISecurityService _securityService;
        protected readonly IMapper _mapper;

        public RegisterUserCommandHandler(ISender mediator, IWriteRepository<User> userWriteRepository,
                                   ISecurityService securityService, IMapper mapper)
        {
            _mediator = mediator;
            _userWriteRepository = userWriteRepository;
            _securityService = securityService;
            _mapper = mapper;
        }

        public async Task<AuthUserDto> Handle(RegisterUserCommand command, CancellationToken _)
        {
            var newUser = _mapper.Map<User>(command.RegisterUser);
            newUser.CompanyId = "1"; // !IMPORTANT! delete in the future         

            newUser.IsEmailConfirmed = false;
            var salt = _securityService.GetRandomBytes();

            newUser.PasswordSalt = Convert.ToBase64String(salt);
            newUser.Password = _securityService.HashPassword(command.RegisterUser.Password, salt);

            await _userWriteRepository.CreateAsync(newUser);

            var registeredUser = _mapper.Map<UserDto>(newUser);
            registeredUser.Roles = command.RegisterUser.Roles;

            var generateTokenCommand = new GenerateAccessTokenCommand(registeredUser);
            var token = await _mediator.Send(generateTokenCommand);

            return new AuthUserDto
            {
                User = registeredUser,
                Token = token
            };
        }
    }
}