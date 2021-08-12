using Application.Auth.Exceptions;
using Domain.Entities;
using Domain.Interfaces.Read;
using Domain.Interfaces.Abstractions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Common.Exceptions;

namespace Application.Auth.Commands
{
    public class LogoutCommand : IRequest<Unit>
    {
        public string RefreshToken { get; }

        public LogoutCommand(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }

    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
    {
        protected readonly IRTokenReadRepository _tokenReadRepository;
        protected readonly IWriteRepository<RefreshToken> _tokenWriteRepository;
        protected readonly ICurrentUserContext _currentUserContext;

        public LogoutCommandHandler(IRTokenReadRepository tokenReadRepository, IWriteRepository<RefreshToken> tokenWriteRepository,
                                    ICurrentUserContext currentUserContext)
        {
            _tokenReadRepository = tokenReadRepository;
            _tokenWriteRepository = tokenWriteRepository;
            _currentUserContext = currentUserContext;
        }

        public async Task<Unit> Handle(LogoutCommand command, CancellationToken _)
        {
            var user = await _currentUserContext.GetCurrentUser();
            if (user is null)
            {
                throw new NotFoundException(typeof(User), "unknown");
            }

            var refreshToken = await _tokenReadRepository.GetAsync(command.RefreshToken, user.Id);

            if (refreshToken == null)
            {
                throw new InvalidTokenException("refresh");
            }

            await _tokenWriteRepository.DeleteAsync(refreshToken.Id);

            return Unit.Value;
        }
    }
}
