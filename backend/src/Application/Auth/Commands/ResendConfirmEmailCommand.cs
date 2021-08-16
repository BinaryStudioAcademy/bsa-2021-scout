using Application.Auth.Commands;
using Application.Auth.Dtos;
using Application.Auth.Exceptions;
using Application.Common.Exceptions;
using Application.Common.Mail;
using Application.Common.Queries;
using Application.Interfaces;
using Application.Mail;
using Application.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public class ResendConfirmEmailCommand : IRequest<Unit>
    {
        public ResendConfirmEmailDto ResendConfirmEmailDto { get; }

        public ResendConfirmEmailCommand(ResendConfirmEmailDto resendConfirmEmailDto)
        {
            ResendConfirmEmailDto = resendConfirmEmailDto;
        }
    }

    public class ResendConfirmEmailCommandHandler : IRequestHandler<ResendConfirmEmailCommand, Unit>
    {
        protected readonly ISender _mediator;
        protected readonly IWriteRepository<EmailToken> _emailTokenRepository;

        protected readonly ISecurityService _securityService;
        protected readonly IMapper _mapper;

        public ResendConfirmEmailCommandHandler(ISender mediator, IWriteRepository<EmailToken> emailTokenRepository,
                                   ISecurityService securityService, IMapper mapper)
        {
            _mediator = mediator;
            _emailTokenRepository = emailTokenRepository;
            _securityService = securityService;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(ResendConfirmEmailCommand command, CancellationToken _)
        {
            var getUserByPropertyQuery = new GetEntityByPropertyQuery<UserDto>("Email", command.ResendConfirmEmailDto.Email);
            var user = await _mediator.Send(getUserByPropertyQuery);

            if (user == null)
            {
                throw new NotFoundException("User with this email is not found");
            }
            if (user.IsEmailConfirmed)
            {
                throw new EmailIsAlreadyConfirmed();
            }
            var deleteEmailTokenCommand = new DeleteEmailTokenCommand(user.Id);
            await _mediator.Send(deleteEmailTokenCommand);

            var sendConfirmEmailMailCommand = new SendConfirmEmailMailCommand(
                user,
                command.ResendConfirmEmailDto.ClientUrl,
                MailSubjectFactory.CONFIRM_EMAIL,
                MailBodyFactory.CONFIRM_EMAIL);
            await _mediator.Send(sendConfirmEmailMailCommand);

            return Unit.Value;

        }
    }
}
