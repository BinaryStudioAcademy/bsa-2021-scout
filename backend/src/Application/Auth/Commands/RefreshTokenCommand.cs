using Application.Auth.Dtos;
using Application.Auth.Exceptions;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
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
        protected readonly IReadRepository<User> _userRepository;
        protected readonly IRTokenReadRepository _tokenReadRepository;
        protected readonly IWriteRepository<RefreshToken> _tokenWriteRepository;

        protected readonly IJwtService _jwtService;

        public RefreshTokenCommandHandler(IReadRepository<User> userRepository, IRTokenReadRepository tokenReadRepository, 
                                   IWriteRepository<RefreshToken> tokenWriteRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _tokenReadRepository = tokenReadRepository;
            _tokenWriteRepository = tokenWriteRepository;
            _jwtService = jwtService;
        }

        public async Task<AccessTokenDto> Handle(RefreshTokenCommand command, CancellationToken _)
        {
            var userId = _jwtService.GetUserIdFromToken(command.Token.AccessToken, command.Token.SigningKey);
            var user = await _userRepository.GetAsync(userId);

            if (user == null)
            {
                throw new NotFoundException(typeof(User), userId);
            }

            var refreshToken = await _tokenReadRepository.GetAsync(command.Token.RefreshToken, userId);
            
            if (refreshToken == null)
            {
                throw new InvalidTokenException("refresh");
            }

            if (!refreshToken.IsActive)
            {
                throw new ExpiredRefreshTokenException();
            }
     
            return await GenerateNewAccessToken(user, refreshToken);
        }

        public async Task<AccessTokenDto> GenerateNewAccessToken(User user, RefreshToken refreshToken)
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
