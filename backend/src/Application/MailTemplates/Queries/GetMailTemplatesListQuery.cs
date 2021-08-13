using System.Collections.Generic;
using MediatR;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Application.Common.Queries;
using Application.MailTemplates.Dtos;

namespace Application.MailTemplates.Queries
{
    public class GetMailTemplatesListQueryHandler
        : GetEntityListQueryHandler<MailTemplate, MailTemplateDto>, IRequestHandler<GetEntityListQuery<MailTemplateDto>, IEnumerable<MailTemplateDto>>
    {
        public GetMailTemplatesListQueryHandler(IReadRepository<MailTemplate> repository, IMapper mapper)
            : base(repository, mapper) { }
    }
}
