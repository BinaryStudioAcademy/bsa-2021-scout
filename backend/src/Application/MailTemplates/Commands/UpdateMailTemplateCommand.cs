using Domain.Entities;
using Application.Common.Commands;
using Domain.Interfaces.Abstractions;
using Application.MailTemplates.Dtos;
using AutoMapper;

namespace Application.MailTemplates.Commands
{
    public class UpdateMailTemplateCommandHandler: UpdateEntityCommandHandler<MailTemplate, MailTemplateDto>
    {
        public UpdateMailTemplateCommandHandler(IWriteRepository<MailTemplate> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
