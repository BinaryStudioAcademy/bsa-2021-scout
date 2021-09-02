using Domain.Entities;
using Application.Common.Commands;
using Domain.Interfaces.Abstractions;
using Application.MailTemplates.Dtos;
using AutoMapper;
using MediatR;
using Application.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using System;
using Application.Common.Exceptions;
using Application.MailAttachments.Dtos;
using Application.MailAttachments.Commands;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Application.MailTemplates.Commands
{
    public class UpdateMailTemplateCommand : IRequest<MailTemplateDto>
    {
        public string Body;
        public List<IFormFile> Files;
        public UpdateMailTemplateCommand(string body, List<IFormFile> files)
        {
            Body = body;
            Files = files;
        }
    }

    public class UpdateMailTemplateCommandHandler : IRequestHandler<UpdateMailTemplateCommand, MailTemplateDto>
    {
        protected readonly IWriteRepository<MailTemplate> _writeRepository;
        protected readonly IReadRepository<MailTemplate> _readRepository;

        protected readonly IMapper _mapper;
        protected readonly ICurrentUserContext _currentUserContext;

        protected readonly ISender _mediator;

        public UpdateMailTemplateCommandHandler(IWriteRepository<MailTemplate> writeRepository,
            IReadRepository<MailTemplate> readRepository,
            IMapper mapper,
            ICurrentUserContext currentUserContext,
            ISender mediator)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
            _mapper = mapper;
            _currentUserContext = currentUserContext;
            _mediator = mediator;
        }

        public async Task<MailTemplateDto> Handle(UpdateMailTemplateCommand command, CancellationToken cancellationToken)
        {
            var mailTemplateUpdateDto = JsonConvert.DeserializeObject<MailTemplateUpdateDto>(command.Body);
            if (mailTemplateUpdateDto.MailAttachments == null)
            {
                mailTemplateUpdateDto.MailAttachments = new List<MailAttachmentUpdateDto>();
            }
            foreach (var file in command.Files)
            {
                mailTemplateUpdateDto.MailAttachments.Add(
                    new MailAttachmentUpdateDto()
                    {
                        Name = file.FileName,
                        File = file
                    });
            }

            var mailTemplate = await _readRepository.GetAsync(mailTemplateUpdateDto.Id);
            if(mailTemplate == null)
            {
                throw new NotFoundException(typeof(MailTemplate), mailTemplateUpdateDto.Id);
            }
            var updatedMailTemplate = _mapper.Map<MailTemplate>(mailTemplateUpdateDto);

            updatedMailTemplate.CompanyId = mailTemplate.CompanyId;
            updatedMailTemplate.DateCreation = mailTemplate.DateCreation;
            updatedMailTemplate.UserCreatedId = mailTemplate.UserCreatedId;
            updatedMailTemplate.UserCreated = mailTemplate.UserCreated;


            var existedmailAttachments = (List<MailAttachment>)mailTemplate.MailAttachments;
            updatedMailTemplate.MailAttachments = new List<MailAttachment>();
            foreach (var mailAttachmentUpdateDto in mailTemplateUpdateDto.MailAttachments)
            {
                if (mailAttachmentUpdateDto.Id == null)
                {
                    var mailAttachment = new MailAttachment()
                    {
                        MailTemplateId = mailTemplate.Id,
                        Name = mailAttachmentUpdateDto.Name
                    };
                    var uploadMailAttachmentFileCommand = new UploadMailAttachmentFileCommand(
                        mailAttachment.Id,
                        mailAttachmentUpdateDto.Id,
                        _mapper.Map<MailAttachmentCreateDto>(mailAttachmentUpdateDto));
                    mailAttachment.Key = await _mediator.Send(uploadMailAttachmentFileCommand);
                    updatedMailTemplate.MailAttachments.Add(mailAttachment);
                }
                else 
                {
                    if (mailAttachmentUpdateDto.File == null)
                    {
                        var mailAttachment = existedmailAttachments.Find(x => x.Id == mailAttachmentUpdateDto.Id);
                        updatedMailTemplate.MailAttachments.Add(mailAttachment);
                        existedmailAttachments.Remove(mailAttachment);
                    }
                }
            }
            if (existedmailAttachments != null
                && existedmailAttachments.Count != 0)
            {
                foreach (var mailAttachment in existedmailAttachments)
                {
                    var deleteMailAttachmentFileCommand = new DeleteMailAttachmentFileCommand(mailAttachment.Key);
                    await _mediator.Send(deleteMailAttachmentFileCommand);
                }
            }

            var created = await _writeRepository.UpdateAsync(updatedMailTemplate);

            return _mapper.Map<MailTemplateDto>(created);
        }
    }
}
