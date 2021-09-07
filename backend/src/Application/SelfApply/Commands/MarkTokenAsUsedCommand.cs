using Application.Common.Exceptions;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SelfApply.Commands
{
    public class MarkTokenAsUsedCommand : IRequest
    {
        public string Token { get; }

        public MarkTokenAsUsedCommand(string token)
        {
            Token = token;
        }
    }

    public class MarkTokenAsUsedCommandHandler : IRequestHandler<MarkTokenAsUsedCommand>
    {
        protected readonly IWriteRepository<ApplyToken> _writeRepository;
        protected readonly IReadRepository<ApplyToken> _readRepository;

        public MarkTokenAsUsedCommandHandler(IWriteRepository<ApplyToken> writeRepository, IReadRepository<ApplyToken> readRepository)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
        }

        public async Task<Unit> Handle(MarkTokenAsUsedCommand command, CancellationToken _)
        {
            var token = await _readRepository.GetByPropertyAsync("token", command.Token);

            if (token is null)
            {
                throw new NotFoundException(nameof(ApplyToken));
            }

            token.IsActive = false;

            await _writeRepository.UpdateAsync(token);

            return Unit.Value;
        }
    }
}
