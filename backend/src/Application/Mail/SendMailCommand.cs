using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;
using Application.Interfaces;

namespace Application.Mail
{
    public class SendMailCommand : IRequest
    {
        public IEnumerable<string> To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string TemplateSlug { get; set; }

        public SendMailCommand(string to, string subject, string body, string templateSlug = "default")
        {
            To = new List<string> { to };
            Subject = subject;
            Body = body;
            TemplateSlug = templateSlug;
        }

        public SendMailCommand(IEnumerable<string> to, string subject, string body, string templateSlug = "default")
        {
            To = to;
            Subject = subject;
            Body = body;
            TemplateSlug = templateSlug;
        }
    }

    public class SendMailCommandHandler : IRequestHandler<SendMailCommand>
    {
        private readonly ISmtpFactory _smtp;

        public SendMailCommandHandler(ISmtpFactory smtp)
        {
            _smtp = smtp;
        }

        public async Task<Unit> Handle(SendMailCommand command, CancellationToken _)
        {
            string address = Environment.GetEnvironmentVariable("MAIL_ADDRESS");
            string password = Environment.GetEnvironmentVariable("MAIL_PASSWORD");
            string displayName = Environment.GetEnvironmentVariable("MAIL_DISPLAY_NAME");

            using (ISmtp connection = _smtp.Connect(address, password, displayName))
            {
                await connection.SendAsync(command.To, command.Subject, command.Body, command.TemplateSlug);
            }

            return Unit.Value;
        }
    }
}
