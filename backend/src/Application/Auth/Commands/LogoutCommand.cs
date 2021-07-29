using Application.Auth.Dtos;
using Application.Auth.Exceptions;
using Application.Common.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands
{
    public class LogoutCommand : IRequest<Unit>
    {
        public string RefreshToken { get; }
        public Guid UserId { get;  }

        public LogoutCommand(string refreshToken, Guid userId)
        {
            RefreshToken = refreshToken;
            UserId = userId;
        }
    }

    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
    {
        protected readonly IRTokenReadRepository _tokenReadRepository;
        protected readonly IWriteRepository<RefreshToken> _tokenWriteRepository;

        public LogoutCommandHandler(IRTokenReadRepository tokenReadRepository, IWriteRepository<RefreshToken> tokenWriteRepository)
        {
            _tokenReadRepository = tokenReadRepository;
            _tokenWriteRepository = tokenWriteRepository;
        }

        public async Task<Unit> Handle(LogoutCommand command, CancellationToken _)
        {
            var refreshToken = await _tokenReadRepository.GetAsync(command.RefreshToken, command.UserId);

            if (refreshToken == null)
            {
                throw new InvalidTokenException("refresh");
            }

            await _tokenWriteRepository.DeleteAsync(Guid.Parse(refreshToken.Id));

            return Unit.Value;
        }
    }
}
