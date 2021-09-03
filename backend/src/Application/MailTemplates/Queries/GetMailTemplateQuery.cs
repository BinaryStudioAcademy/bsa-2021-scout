using System.Collections.Generic;
using MediatR;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Application.Common.Queries;
using Application.MailTemplates.Dtos;


namespace Application.MailTemplates.Queries
{
    public class GetMailTemplateQueryHandler
        : GetEntitiesQueryHandler<MailTemplate, MailTemplateDto>, IRequestHandler<GetEntityByIdQuery<MailTemplateDto>, MailTemplateDto>
    {
        public GetMailTemplateQueryHandler(IReadRepository<MailTemplate> repository, IMapper mapper)
            : base(repository, mapper) { }
    }
}