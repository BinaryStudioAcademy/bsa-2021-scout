using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;
using Application.Interfaces;
using System.Net.Mail;

namespace Application.Mail
{
    public class SendMailCommand : IRequest
    {
        public IEnumerable<string> To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string TemplateSlug { get; set; }
        public ICollection<Attachment> Attachments { get; set; }

        public SendMailCommand(string to, string subject, string body, string templateSlug = "default", ICollection<Attachment> attachments = null)
        {
            To = new List<string> { to };
            Subject = subject;
            Body = body;
            TemplateSlug = templateSlug;
            Attachments = attachments;
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
            using (ISmtp connection = await _smtp.Connect())
            {
                await connection.SendAsync(command.To, command.Subject, command.Body, command.TemplateSlug, command.Attachments);
            }

            return Unit.Value;
        }
    }
}
