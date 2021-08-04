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
        private readonly ISmtp _smtp;

        public SendMailCommandHandler(ISmtp smtp)
        {
            _smtp = smtp;
        }

        public async Task<Unit> Handle(SendMailCommand command, CancellationToken _)
        {
            await _smtp.SendAsync(command.To, command.Subject, command.Body, command.TemplateSlug);

            return Unit.Value;
        }
    }
}
