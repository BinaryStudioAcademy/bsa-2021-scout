using System.Collections.Generic;
using MediatR;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Application.Common.Queries;
using Application.MailAttachments.Dtos;
using System.Threading.Tasks;
using System.Threading;
using Domain.Interfaces.Read;

namespace Application.MailAttachments.Queries
{
    public class GetMailAttachmentsByMailTempalteIdQuery : IRequest<ICollection<MailAttachmentDto>>
    {
        public string MailTemplateId { get; }
        public GetMailAttachmentsByMailTempalteIdQuery(string mailTemplateId)
        {
            MailTemplateId = mailTemplateId;
        }
    }
    public class GetMailAttachmentByMailTempalteIdQueryHandler
        : IRequestHandler<GetMailAttachmentsByMailTempalteIdQuery, ICollection<MailAttachmentDto>>
    {
        protected readonly IMailAttachmentReadRepository _readRepository;
        protected readonly IMapper _mapper;


        public GetMailAttachmentByMailTempalteIdQueryHandler(IMailAttachmentReadRepository readRepository,
            IMapper mapper)
        {
            _readRepository = readRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<MailAttachmentDto>> Handle(GetMailAttachmentsByMailTempalteIdQuery query, CancellationToken cancellationToken)
        {
            return _mapper.Map<ICollection<MailAttachmentDto>>(await _readRepository.GetByMailTemplateIdAsync(query.MailTemplateId));
        }
    }
}
