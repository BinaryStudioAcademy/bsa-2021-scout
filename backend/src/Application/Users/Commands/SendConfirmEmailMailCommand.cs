using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Application.Interfaces;
using Application.Auth.Commands;
using Application.Users.Dtos;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using Application.Auth.Dtos;
using AutoMapper;

namespace Application.Mail
{
    public class SendConfirmEmailMailCommand : SendMailCommand
    {
        public UserDto UserDto { get; set; }

        public string ClientUrl { get; set; }


        public SendConfirmEmailMailCommand(UserDto userDto, string clientUrl, string subject, string body, string templateSlug = "default") : base(userDto.Email, subject, body, templateSlug = "default")
        {
            UserDto = userDto;
            ClientUrl = clientUrl;
        }

        public class SendConfirmEmailMailCommandHandler : IRequestHandler<SendConfirmEmailMailCommand>
        {
            private readonly ISmtpFactory _smtp;
            protected readonly ISender _mediator;

            public SendConfirmEmailMailCommandHandler(ISender mediator, ISmtpFactory smtp)
            {
                _smtp = smtp;
                _mediator = mediator;
            }

            public async Task<Unit> Handle(SendConfirmEmailMailCommand command, CancellationToken _)
            {
                var emailTokenCommand = new GenerateEmailTokenCommand(command.UserDto);

                var queryParam = new Dictionary<string, string>
                {
                    {"email", command.UserDto.Email },
                    {"token", (await _mediator.Send(emailTokenCommand)).Token },
                };

                var callbackUrl = QueryHelpers.AddQueryString(command.ClientUrl, queryParam);

                using (ISmtp connection = await _smtp.Connect())
                {
                    await connection.SendAsync(command.To, command.Subject, command.Body.Replace("{{LINK}}", callbackUrl), command.TemplateSlug);
                }

                return Unit.Value;
            }
        }
    }
}
