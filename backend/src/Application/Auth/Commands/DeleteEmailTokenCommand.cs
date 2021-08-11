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
using Application.Common.Queries;

namespace Application.Auth.Commands
{
    public class DeleteEmailTokenCommand : IRequest<Unit>
    {
        public string UserId { get; }

        public DeleteEmailTokenCommand(string userId)
        {
            UserId = userId;
        }
    }

    public class DeleteEmailTokenCommandHandler : IRequestHandler<DeleteEmailTokenCommand, Unit>
    {
        protected readonly IReadRepository<EmailToken> _tokenReadRepository;
        protected readonly IWriteRepository<EmailToken> _tokenWriteRepository;

        protected readonly ISender _mediator;


        public DeleteEmailTokenCommandHandler(IReadRepository<EmailToken> tokenReadRepository,
                                   IWriteRepository<EmailToken> tokenWriteRepository,
                                   ISender mediator)
        {
            _tokenReadRepository = tokenReadRepository;
            _tokenWriteRepository = tokenWriteRepository;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(DeleteEmailTokenCommand command, CancellationToken _)
        {
            var getEntityByPropertyQuery = new GetEntityByPropertyQuery<EmailTokenDto>("UserId", command.UserId);
            var emailToken = await _mediator.Send(getEntityByPropertyQuery);

            await _tokenWriteRepository.DeleteAsync(emailToken.Id);

            return Unit.Value;
        }
    }
}
