using Application.Common.Mail;
using Application.Interfaces;
using Application.Mail;
using Application.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands
{
    public class ResendRegistrationLinkCommand : IRequest<RegisterPermissionShortDto>
    {
        public RegisterPermissionShortDto Dto { get; private set; }

        public ResendRegistrationLinkCommand(RegisterPermissionShortDto registerPermissionShortDto)
        {
            Dto = registerPermissionShortDto;
        }
    }

    public class ResendRegistrationLinkCommandHandler : IRequestHandler<ResendRegistrationLinkCommand, RegisterPermissionShortDto>
    {
        private readonly IWriteRepository<RegisterPermission> _registerPermissionWriteRepository;
        private readonly IReadRepository<RegisterPermission> _registerPermissionReadRepository;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public ResendRegistrationLinkCommandHandler(
            IWriteRepository<RegisterPermission> registerPermissionWriteRepository,
            IReadRepository<RegisterPermission> registerPermissionReadRepository,
            ICurrentUserContext currentUserContext,
            ISender mediator,
            IMapper mapper)
        {
            _registerPermissionReadRepository = registerPermissionReadRepository;
            _registerPermissionWriteRepository = registerPermissionWriteRepository;
            _currentUserContext = currentUserContext;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<RegisterPermissionShortDto> Handle(ResendRegistrationLinkCommand command, CancellationToken _)
        {
            await _registerPermissionWriteRepository.DeleteAsync(command.Dto.Id);

            var registerPermission = new RegisterPermission()
            {
                Email = command.Dto.Email,
                CompanyId = (await _currentUserContext.GetCurrentUser()).CompanyId
            };

            var newPermissionEntity = await _registerPermissionWriteRepository.CreateAsync(registerPermission);

            var queryParam = new Dictionary<string, string>
            {
                {"email", command.Dto.Email }
            };

            var clientUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") + "/registration";
            var callback = QueryHelpers.AddQueryString(clientUrl, queryParam);
            var body = MailBodyFactory.BODY_REGISTRATION_LINK;
            body = body.Replace("{{CALLBACK}}", callback);
            var sendMailCommand = new SendMailCommand(command.Dto.Email, MailSubjectFactory.SUBJECT_REGISTRATION_LINK, body);
            await _mediator.Send(sendMailCommand);

            return _mapper.Map<RegisterPermissionShortDto>(newPermissionEntity as RegisterPermission);
        }
    }
}
