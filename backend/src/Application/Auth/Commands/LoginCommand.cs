using Application.Auth.Dtos;
using Application.Auth.Exceptions;
using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Read;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands
{
    public class LoginCommand : IRequest<AuthUserDto>
    {
        public string Email { get; }
        public string Password { get; }

        public LoginCommand(UserLoginDto loginUser)
        {
            Email = loginUser.Email;
            Password = loginUser.Password;
        }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthUserDto>
    {
        protected readonly ISender _mediator;
        protected readonly IUserReadRepository _userRepository;

        protected readonly ISecurityService _securityService;
        protected readonly IMapper _mapper;

        public LoginCommandHandler(ISender mediator, IUserReadRepository userRepository,
                                   ISecurityService securityService, IMapper mapper)
        {
            _mediator = mediator;
            _userRepository = userRepository;
            _securityService = securityService;
            _mapper = mapper;
        }

        public async Task<AuthUserDto> Handle(LoginCommand command, CancellationToken _)
        {
            User user = await _userRepository.GetByEmailAsync(command.Email);

            if (user == null)
            {
                throw new NotFoundException($"Can't find user with email {command.Email}.");
            }

            await _userRepository.LoadRolesAsync(user);

            if (!_securityService.ValidatePassword(command.Password, user.Password, user.PasswordSalt))
            {
                throw new InvalidUsernameOrPasswordException();
            }

            UserDto userDto = _mapper.Map<UserDto>(user);

            var generateTokenCommand = new GenerateAccessTokenCommand(userDto);
            var token = await _mediator.Send(generateTokenCommand);

            return new AuthUserDto
            {
                User = userDto,
                Token = token
            };
        }
    }
}
