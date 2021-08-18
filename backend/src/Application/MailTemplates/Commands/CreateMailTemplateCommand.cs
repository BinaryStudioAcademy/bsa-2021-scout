using Application.Common.Commands;
using Application.MailTemplates.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MailTemplates.Commands
{
    public class CreateMailTemplateCommandHandler : CreateEntityCommandHandler<MailTemplate, MailTemplateDto>
    {
        public CreateMailTemplateCommandHandler(IWriteRepository<MailTemplate> repository, IMapper mapper) : base(repository, mapper) { }

    }
}
