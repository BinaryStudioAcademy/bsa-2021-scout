using System.Collections.Generic;
using MediatR;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Application.Common.Queries;
using Application.MailTemplates.Dtos;
using System.Threading.Tasks;
using System.Threading;
using Application.Interfaces.AWS;
using Domain.Interfaces.Read;
using System.Net.Mail;
using System.IO;

namespace Application.MailTemplates.Queries
{

    public class GetMailWithAttachmentFilesTemplateQuery : IRequest<MailTemplateSendDto>
    {
        public string Id;
        public GetMailWithAttachmentFilesTemplateQuery(string id)
        {
            Id = id;
        }
}
    public class GetMailWithAttachmentFilesTemplateQueryyHandler : IRequestHandler<GetMailWithAttachmentFilesTemplateQuery, MailTemplateSendDto>
    {
        protected readonly IAwsS3ReadRepository _readAws3Repository;
        protected readonly IMailTemplateReadRepository _readMailTemplateRepository;
        protected readonly IMapper _mapper;
        public GetMailWithAttachmentFilesTemplateQueryyHandler(
            IAwsS3ReadRepository readAws3Repository,
            IMailTemplateReadRepository readMailTemplateRepository,
            IMapper mapper)
        {
            _readAws3Repository = readAws3Repository;
            _readMailTemplateRepository = readMailTemplateRepository;
            _mapper = mapper;
        }
        public async Task<MailTemplateSendDto> Handle(GetMailWithAttachmentFilesTemplateQuery query, CancellationToken cancellationToken)
        {
            var mailTemplate = await _readMailTemplateRepository.GetAsync(query.Id);
            var mailTemplateSendDto = _mapper.Map<MailTemplateSendDto>(mailTemplate);
            if (mailTemplate.MailAttachments != null && mailTemplate.MailAttachments.Count != 0)
            {
                mailTemplateSendDto.Attachments = new List<Attachment>();
                foreach (var mailAttachment in mailTemplate.MailAttachments)
                {
                    var file = await _readAws3Repository.ReadAsync(mailAttachment.Key);
                    MemoryStream memoryStream = new MemoryStream(file);
                    mailTemplateSendDto.Attachments.Add(new Attachment(memoryStream, mailAttachment.Name));
                }
            }
            return mailTemplateSendDto; 
        }
    }
}