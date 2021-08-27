using Application.Common.Commands;
using Application.Interfaces;
using Application.MailTemplates.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.MailTemplates.Commands
{
    public class CreateMailTemplateCommand : IRequest<MailTemplateDto>
    {
        public MailTemplateCreateDto MailTemplateDto { get; }
        public CreateMailTemplateCommand(MailTemplateCreateDto mailTemplateDto)
        {
            MailTemplateDto = mailTemplateDto;
        }
    }

    public class CreateMailTemplateCommandHandler : IRequestHandler<CreateMailTemplateCommand, MailTemplateDto> 
    {
        protected readonly IWriteRepository<MailTemplate> _repository;
        protected readonly IMapper _mapper;
        protected readonly ICurrentUserContext _currentUserContext;
        protected readonly ISender _mediator;

        public CreateMailTemplateCommandHandler(IWriteRepository<MailTemplate> repository, 
            IMapper mapper, 
            ICurrentUserContext currentUserContext,
            ISender mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _currentUserContext = currentUserContext;
            _mediator = mediator;
        }

        public async Task<MailTemplateDto> Handle(CreateMailTemplateCommand command, CancellationToken cancellationToken)
        {

            var entity = new MailTemplate();

            var currentUser = await _currentUserContext.GetCurrentUser();

            entity.Slug = command.MailTemplateDto.Slug;
            entity.Subject = command.MailTemplateDto.Subject;
            entity.VisibilitySetting = entity.VisibilitySetting;
            entity.Html = entity.Html;
            entity.UserCreatedId = currentUser.Id;
            entity.CompanyId = currentUser.CompanyId;
            entity.DateCreation = DateTime.UtcNow;

            foreach (var mailAttachmentDto in command.MailTemplateDto.MailAttachments)
            {
                var mailAttachment = new MailAttachment()
                {
                    MailTemplateId = entity.Id,
                    Name = mailAttachmentDto.Name
                };
                var uploadMailAttachmentFileCommand = new UploadMailAttachmentFileCommand(
                    mailAttachment.Id,
                    entity.Id,
                    mailAttachmentDto);
                mailAttachment.Key = await _mediator.Send(uploadMailAttachmentFileCommand);
                entity.MailAttachments.Add(mailAttachment);
            }

            var created = await _repository.CreateAsync(entity);

            return _mapper.Map<MailTemplateDto>(created);
        }
    }
}
