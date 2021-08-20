using Application.Auth.Dtos;
using Application.Auth.Exceptions;
using Application.Common.Mail;
using Application.Interfaces;
using Application.Mail;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands
{
    public class SendRegistrationLinkCommand : IRequest<Unit>
    {
        public RegistrationLinkDto RegistrationLinkDto { get; }
        public SendRegistrationLinkCommand(RegistrationLinkDto registrationLinkDto)
        {
            RegistrationLinkDto = registrationLinkDto;
        }
    }

    public class SendRegistrationLinkCommandHandler : IRequestHandler<SendRegistrationLinkCommand, Unit>
    {
        protected readonly ICurrentUserContext _currentUserContext;
        protected readonly IReadRepository<RegisterPermission> _registerPermissionReadRepository;
        protected readonly IWriteRepository<RegisterPermission> _registerPermissionWriteRepository;
        protected readonly ISender _mediator;

        public SendRegistrationLinkCommandHandler(ICurrentUserContext currentUserContext,
                                            IReadRepository<RegisterPermission> registerPermissionReadRepository,
                                            IWriteRepository<RegisterPermission> registerPermissionWriteRepository,
                                            ISender mediator)
        {
            _currentUserContext = currentUserContext;
            _registerPermissionReadRepository = registerPermissionReadRepository;
            _registerPermissionWriteRepository = registerPermissionWriteRepository;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(SendRegistrationLinkCommand command, CancellationToken _)
        {
            var hrLead = await _currentUserContext.GetCurrentUser();

            var registerPermission = await _registerPermissionReadRepository.GetByPropertyAsync(nameof(RegisterPermission.Email), command.RegistrationLinkDto.Email);
            if (registerPermission is not null)
            {
                await _registerPermissionWriteRepository.DeleteAsync(registerPermission.Id);
            }
            registerPermission = new RegisterPermission()
            {
                Email = command.RegistrationLinkDto.Email,
                CompanyId = hrLead.CompanyId
            };
            await _registerPermissionWriteRepository.CreateAsync(registerPermission);

            var queryParam = new Dictionary<string, string>
            {
                {"email", command.RegistrationLinkDto.Email }
            };

            var callback = QueryHelpers.AddQueryString(command.RegistrationLinkDto.ClientUri, queryParam);
            var body = MailBodyFactory.BODY_REGISTRATION_LINK;
            body = body.Replace("{{CALLBACK}}", callback);
            var sendMailCommand = new SendMailCommand(command.RegistrationLinkDto.Email, MailSubjectFactory.SUBJECT_REGISTRATION_LINK, body);
            await _mediator.Send(sendMailCommand);

            return Unit.Value;
        }
    }
}
