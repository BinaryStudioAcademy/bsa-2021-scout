using Domain.Entities;
using Application.Common.Commands;
using Domain.Interfaces.Abstractions;

namespace Application.MailTemplates.Commands
{
    public class DeleteMailTemplateCommandHandler : DeleteEntityCommandHandler<MailTemplate>
    {
        public DeleteMailTemplateCommandHandler(IWriteRepository<MailTemplate> repository)
        : base(repository)
        { }
    }
}
