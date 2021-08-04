using Application.Auth.Dtos;
using Application.Auth.Exceptions;
using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Read;
using Domain.Interfaces.Abstractions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands
{
    public class RefreshTokenCommand : IRequest<AccessTokenDto>
    {
        public RefreshTokenDto Token { get; }

        public RefreshTokenCommand(RefreshTokenDto token)
        {
            Token = token;
        }
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AccessTokenDto>
    {
        protected readonly IUserReadRepository _userRepository;
        protected readonly IRTokenReadRepository _tokenReadRepository;
        protected readonly IWriteRepository<RefreshToken> _tokenWriteRepository;

        protected readonly IMapper _mapper;
        protected readonly IJwtService _jwtService;

        public RefreshTokenCommandHandler(IUserReadRepository userRepository, IRTokenReadRepository tokenReadRepository,
                                   IWriteRepository<RefreshToken> tokenWriteRepository, IJwtService jwtService,
                                   IMapper mapper)
        {
            _userRepository = userRepository;
            _tokenReadRepository = tokenReadRepository;
            _tokenWriteRepository = tokenWriteRepository;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task<AccessTokenDto> Handle(RefreshTokenCommand command, CancellationToken _)
        {
            var userId = _jwtService.GetUserIdFromToken(command.Token.AccessToken, command.Token.SigningKey);
            var user = await _userRepository.GetAsync(userId);

            if (user == null)
            {
                throw new NotFoundException(typeof(User), userId);
            }

            await _userRepository.LoadRolesAsync(user);

            var refreshToken = await _tokenReadRepository.GetAsync(command.Token.RefreshToken, userId);

            if (refreshToken == null)
            {
                throw new InvalidTokenException("refresh");
            }

            if (!refreshToken.IsActive)
            {
                throw new ExpiredRefreshTokenException();
            }

            var userDto = _mapper.Map<UserDto>(user);
            return await GenerateNewAccessToken(userDto, refreshToken);
        }

        public async Task<AccessTokenDto> GenerateNewAccessToken(UserDto user, RefreshToken refreshToken)
        {
            await _tokenWriteRepository.DeleteAsync(refreshToken.Id); // delete the token we've exchanged

            var newRefreshToken = _jwtService.GenerateRefreshToken();
            await _tokenWriteRepository.CreateAsync(new RefreshToken   // add the new one
            {
                Token = newRefreshToken,
                UserId = user.Id
            });

            var accessToken = await _jwtService.GenerateJsonWebToken(user);

            return new AccessTokenDto(accessToken, newRefreshToken);
        }
    }
}
