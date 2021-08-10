using Application.Auth.Dtos;
using Application.Interfaces;
using Application.Users.Dtos;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands
{
    public class GenerateAccessTokenCommand : IRequest<AccessTokenDto>
    {
        public UserDto User { get; }

        public GenerateAccessTokenCommand(UserDto user)
        {
            User = user;
        }
    }

    public class GenerateAccessTokenCommandHandler : IRequestHandler<GenerateAccessTokenCommand, AccessTokenDto>
    {
        protected readonly IWriteRepository<RefreshToken> _tokenRepository;
        protected readonly IJwtService _jwtService;

        public GenerateAccessTokenCommandHandler(IWriteRepository<RefreshToken> tokenRepository, IJwtService jwtService)
        {
            _tokenRepository = tokenRepository;
            _jwtService = jwtService;
        }

        public async Task<AccessTokenDto> Handle(GenerateAccessTokenCommand command, CancellationToken _)
        {
            var refreshToken = _jwtService.GenerateRefreshToken();

            await _tokenRepository.CreateAsync(new RefreshToken
            {
                Token = refreshToken,
                UserId = command.User.Id
            });

            var accessToken = await _jwtService.GenerateJsonWebToken(command.User);

            return new AccessTokenDto(accessToken, refreshToken);
        }
    }
}
