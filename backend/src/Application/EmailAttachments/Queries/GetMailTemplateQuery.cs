using System.Collections.Generic;
using MediatR;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Application.Common.Queries;
using Application.MailAttachments.Dtos;


namespace Application.MailAttachments.Queries
{
    public class GetMailAttachmentQueryHandler
        : GetEntitiesQueryHandler<MailAttachment, MailAttachmentDto>, IRequestHandler<GetEntityByIdQuery<MailAttachmentDto>, MailAttachmentDto>
    {
        public GetMailAttachmentQueryHandler(IReadRepository<MailAttachment> repository, IMapper mapper)
            : base(repository, mapper) { }
    }
}
