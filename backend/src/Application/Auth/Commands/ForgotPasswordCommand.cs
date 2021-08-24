using Application.Auth.Dtos;
using Application.Common.Exceptions;
using Application.Common.Mail;
using Application.Interfaces;
using Application.Mail;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands
{
    public class ForgotPasswordCommand : IRequest<Unit>
    {
        public ForgotPasswordDto ForgotPasswordInfo { get; }
        public ForgotPasswordCommand(ForgotPasswordDto forgotPasswordDto)
        {
            ForgotPasswordInfo = forgotPasswordDto;
        }
    }

    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Unit>
    {
        protected readonly IUserReadRepository _userReadRepository;
        protected readonly IWriteRepository<User> _userWriteRepository;
        protected readonly ISecurityService _securityService;
        protected readonly ISender _mediator;

        public ForgotPasswordCommandHandler(IUserReadRepository userReadRepository, IWriteRepository<User> userWriteRepository,
                                            ISecurityService securityService, ISender mediator)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
            _securityService = securityService;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ForgotPasswordCommand command, CancellationToken _)
        {
            var user = await _userReadRepository.GetByEmailAsync(command.ForgotPasswordInfo.Email);

            if (user is null)
            {
                throw new NotFoundException(nameof(User));
            }

            var resetPasswordToken = Convert.ToBase64String(_securityService.GetRandomBytes());
            user.ResetPasswordToken = resetPasswordToken;
            await _userWriteRepository.UpdateAsync(user);

            var queryParam = new Dictionary<string, string>
            {
                {"token", resetPasswordToken },
                {"email", command.ForgotPasswordInfo.Email }
            };

            var callback = QueryHelpers.AddQueryString(command.ForgotPasswordInfo.ClientUri, queryParam);
            var body = MailBodyFactory.FORGOT_PASSWORD;
            body = body.Replace("{{CALLBACK}}", callback);
            var sendMailCommand = new SendMailCommand(command.ForgotPasswordInfo.Email, MailSubjectFactory.FORGOT_PASSWORD, body);
            await _mediator.Send(sendMailCommand);

            return Unit.Value;
        }
    }
}
