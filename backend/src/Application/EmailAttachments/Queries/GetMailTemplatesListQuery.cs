using System.Collections.Generic;
using MediatR;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Application.Common.Queries;
using Application.MailAttachments.Dtos;

namespace Application.MailAttachments.Queries
{
    public class GetMailAttachmentsListQueryHandler
        : GetEntityListQueryHandler<MailAttachment, MailAttachmentDto>, IRequestHandler<GetEntityListQuery<MailAttachmentDto>, IEnumerable<MailAttachmentDto>>
    {
        public GetMailAttachmentsListQueryHandler(IReadRepository<MailAttachment> repository, IMapper mapper)
            : base(repository, mapper) { }
    }
}
