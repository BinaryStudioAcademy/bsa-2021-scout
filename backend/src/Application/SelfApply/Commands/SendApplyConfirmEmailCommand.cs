using Application.Common.Exceptions;
using Application.Common.Mail;
using Application.Interfaces;
using Application.Mail;
using Application.SelfApply.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SelfApply.Commands
{
    public class SendApplyConfirmEmailCommand : IRequest<Unit>
    {
        public ApplyTokenDto ApplyTokenInfo { get; }
        public SendApplyConfirmEmailCommand(ApplyTokenDto applyTokenDto)
        {
            ApplyTokenInfo = applyTokenDto;
        }
    }

    public class SendApplyConfirmEmailCommandHandler : IRequestHandler<SendApplyConfirmEmailCommand, Unit>
    {
        protected readonly IVacancyReadRepository _vacancyReadRepository;
        protected readonly ISecurityService _securityService;
        protected readonly IWriteRepository<ApplyToken> _writeRepository;
        protected readonly IMapper _mapper;
        protected readonly ISender _mediator;

        public SendApplyConfirmEmailCommandHandler(
            IVacancyReadRepository vacancyReadRepository,
            ISecurityService securityService,
            IWriteRepository<ApplyToken> writeRepository,
            IMapper mapper,
        ISender mediator)
        {
            _vacancyReadRepository = vacancyReadRepository;
            _securityService = securityService;
            _writeRepository = writeRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(SendApplyConfirmEmailCommand command, CancellationToken _)
        {
            var vacancy = await _vacancyReadRepository.GetAsync(command.ApplyTokenInfo.VacancyId);

            if (vacancy is null)
            {
                throw new NotFoundException(nameof(Vacancy));
            }

            var confirmEmailToken = Convert.ToBase64String(_securityService.GetRandomBytes());
            
            var applyToken = _mapper.Map<ApplyToken>(command.ApplyTokenInfo);
            applyToken.Token = confirmEmailToken;
            applyToken.IsActive = true;
            await _writeRepository.CreateAsync(applyToken);

            var queryParam = new Dictionary<string, string>
            {
                {"token", confirmEmailToken },
                {"email", command.ApplyTokenInfo.Email }
            };

            var callback = QueryHelpers.AddQueryString(command.ApplyTokenInfo.ClientUri, queryParam);
            var body = MailBodyFactory.CONFIRM_APPLY_VACANCY;
            body = body.Replace("{{VACANCY}}", vacancy.Title);
            body = body.Replace("{{CALLBACK}}", callback);
            var sendMailCommand = new SendMailCommand(command.ApplyTokenInfo.Email, MailSubjectFactory.CONFIRM_APPLY_VACANCY, body);
            await _mediator.Send(sendMailCommand);

            return Unit.Value;
        }
    }
}
