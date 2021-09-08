using Application.Mail;
using Application.MailTemplates.Queries;
using Domain.Entities;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.MailTemplates.Commands
{
    public class SendEmailWithTemplateCommand : IRequest
    {
        public string Id;
        public string Email;
        public string DataJson;

        public SendEmailWithTemplateCommand(string id, string email, string dataJson)
        {
            Id = id;
            Email = email;
            DataJson = dataJson;
        }
    }

    public class SendEmailCommandHandler : IRequestHandler<SendEmailWithTemplateCommand>
    {
        protected readonly ISender _mediator;

        public SendEmailCommandHandler(
            ISender mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(SendEmailWithTemplateCommand command, CancellationToken cancellationToken)
        {
            var data = JsonConvert.DeserializeObject<jsonData>(command.DataJson);
            var vacancy = JsonConvert.DeserializeObject<Vacancy>(data.vacancy);
            var project = JsonConvert.DeserializeObject<Project>(data.project);
            var applicant = JsonConvert.DeserializeObject<Applicant>(data.applicant);

            if (vacancy == null)
            {
                vacancy = new Vacancy();
            }
            if (project == null)
            {
                project = new Project();
            }
            if (applicant == null)
            {
                applicant = new Applicant();
            }

            vacancy.Project = project;
            var query = new GetMailTemplateWithReplacedPlaceholdersQuery(command.Id, vacancy, applicant);
            var template = await _mediator.Send(query);

            var body = template.Html;
            var sendMailCommand = new SendMailCommand(command.Email, template.Subject, body, attachments: template.Attachments);
            await _mediator.Send(sendMailCommand);

            return Unit.Value;
        }

        class jsonData
        {
            public string? vacancy { get; set; }
            public string? project { get; set; }
            public string? applicant { get; set; }
        }
    }
}
