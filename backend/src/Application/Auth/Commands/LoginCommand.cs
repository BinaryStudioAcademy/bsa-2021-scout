using Application.Auth.Dtos;
using Application.Auth.Exceptions;
using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
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
                throw new NotFoundException(nameof(User));
            }

            if (!_securityService.ValidatePassword(command.Password, user.Password, user.PasswordSalt))
            {
                throw new InvalidUsernameOrPasswordException();
            }

            var generateTokenCommand = new GenerateAccessTokenCommand(user);
            var token = await _mediator.Send(generateTokenCommand);

            return new AuthUserDto
            {
                User = _mapper.Map<UserDto>(user),
                Token = token
            };
        }
    }
    //public class LoginCommand: IRequest<AuthUserDto>
    //{
    //    public string Email { get; }
    //    public string Password { get; }

    //    public LoginCommand(UserLoginDto loginUser)
    //    {
    //        Email = loginUser.Email;
    //        Password = loginUser.Password;
    //    }
    //}

    //public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthUserDto>
    //{
    //    protected readonly IUserReadRepository _userRepository;
    //    protected readonly IWriteRepository<RefreshToken> _tokenRepository;

    //    protected readonly IJwtService _jwtService;
    //    protected readonly ISecurityService _securityService;
    //    protected readonly IMapper _mapper;

    //    public LoginCommandHandler(IUserReadRepository userRepository, IWriteRepository<RefreshToken> tokenRepository,
    //                               IJwtService jwtService, ISecurityService securityService, IMapper mapper)
    //    {
    //        _userRepository = userRepository;
    //        _tokenRepository = tokenRepository;
    //        _jwtService = jwtService;
    //        _securityService = securityService;
    //        _mapper = mapper;
    //    }

    //    public async Task<AuthUserDto> Handle(LoginCommand command, CancellationToken _)
    //    {           
    //        User user = await _userRepository.GetByEmailAsync(command.Email);

    //        if (user == null)
    //        {
    //            throw new NotFoundException(nameof(User));
    //        }

    //        if (!_securityService.ValidatePassword(command.Password, user.Password, user.PasswordSalt))
    //        {
    //            throw new InvalidUsernameOrPasswordException();
    //        }

    //        var token = await GenerateAccessToken(user);

    //        return new AuthUserDto
    //        {
    //            User = _mapper.Map<UserDto>(user),
    //            Token = token
    //        };          
    //    }

    //    public async Task<AccessTokenDto> GenerateAccessToken(User user)
    //    {
    //        var refreshToken = _jwtService.GenerateRefreshToken();

    //        await _tokenRepository.CreateAsync(new RefreshToken
    //        {
    //            Token = refreshToken,
    //            UserId = user.Id 
    //        });

    //        var accessToken = await _jwtService.GenerateJsonWebToken(user);

    //        return new AccessTokenDto(accessToken, refreshToken);
    //    }
    //}
}
