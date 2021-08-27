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

namespace Application.MailTemplates.Commands
{
    public class UpdateMailTemplateCommand : IRequest<MailTemplateDto>
    {
        public MailTemplateUpdateDto MailTemplateDto { get; }
        public UpdateMailTemplateCommand(MailTemplateUpdateDto mailTemplateDto)
        {
            MailTemplateDto = mailTemplateDto;
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
            var mailTemplate = await _readRepository.GetAsync(command.MailTemplateDto.Id);
            if(mailTemplate == null)
            {
                throw new NotFoundException(typeof(MailTemplate), command.MailTemplateDto.Id);
            }
            var updatedMailTemplate = new MailTemplate();

            updatedMailTemplate.Id = mailTemplate.Id;
            updatedMailTemplate.Html = command.MailTemplateDto.Html;
            updatedMailTemplate.Slug = command.MailTemplateDto.Slug;
            updatedMailTemplate.Subject = command.MailTemplateDto.Subject;
            updatedMailTemplate.CompanyId = mailTemplate.CompanyId;
            updatedMailTemplate.DateCreation = mailTemplate.DateCreation;
            updatedMailTemplate.UserCreatedId = mailTemplate.UserCreatedId;

            var existedmailAttachments = mailTemplate.MailAttachments;
            foreach (var mailAttachmentUpdateDto in command.MailTemplateDto.MailAttachments)
            {
                if (mailAttachmentUpdateDto.Id == "")
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
                        var mailAttachment = _mapper.Map<MailAttachment>(mailAttachmentUpdateDto);
                        updatedMailTemplate.MailAttachments.Add(mailAttachment);
                        existedmailAttachments.Remove(mailAttachment);
                    }
                }
            }
            foreach (var mailAttachment in existedmailAttachments)
            {
                var deleteMailAttachmentFileCommand = new DeleteMailAttachmentFileCommand(mailAttachment.Key);
                await _mediator.Send(deleteMailAttachmentFileCommand);
            }

            var created = await _writeRepository.UpdateAsync(updatedMailTemplate);

            return _mapper.Map<MailTemplateDto>(created);
        }
    }
}
