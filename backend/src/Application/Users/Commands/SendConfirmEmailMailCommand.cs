using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Application.Interfaces;
using Application.Auth.Commands;
using Application.Users.Dtos;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;

namespace Application.Mail
{
    public class SendConfirmEmailMailCommand : SendMailCommand
    {
        public UserDto userDTO { get; set; }
        public SendConfirmEmailMailCommand(UserDto userDTO, string subject, string body, string templateSlug = "default") : base(userDTO.Email, subject, body, templateSlug = "default") {
            this.userDTO = userDTO;
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
                string address = Environment.GetEnvironmentVariable("MAIL_ADDRESS");
                string password = Environment.GetEnvironmentVariable("MAIL_PASSWORD");
                string displayName = Environment.GetEnvironmentVariable("MAIL_DISPLAY_NAME");

                var emailTokenCommand = new GenerateEmailTokenCommand(command.userDTO);

                var queryParam = new Dictionary<string, string>
                {
                    {"email", command.userDTO.Email },
                    {"token", (await _mediator.Send(emailTokenCommand)).Token },
                };

                var callbackUrl = QueryHelpers.AddQueryString("https://localhost:5001/api/Register", queryParam);

                using (ISmtp connection = _smtp.Connect(address, password, displayName))
                {
                    await connection.SendAsync(command.To, command.Subject, command.Body.Replace("{{LINK}}", callbackUrl), command.TemplateSlug);
                }

                return Unit.Value;
            }
        }
    }
}
