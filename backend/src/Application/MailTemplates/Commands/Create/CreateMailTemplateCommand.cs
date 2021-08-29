using Application.Common.Commands;
using Application.Interfaces;
using Application.MailAttachments.Dtos;
using Application.MailTemplates.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
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
        public IFormCollection Collection { get; }
        public CreateMailTemplateCommand(IFormCollection collection)
        {
            Collection = collection;
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

            var mailTemplateCreateDto = JsonConvert.DeserializeObject<MailTemplateCreateDto>(command.Collection.Keys.First());

            foreach (var file in command.Collection.Files)
            {
                mailTemplateCreateDto.MailAttachments.Add(
                    new MailAttachmentCreateDto()
                    {
                        Name = file.FileName,
                        File = file
                    });
            }

            var entity = new MailTemplate();

            var currentUser = await _currentUserContext.GetCurrentUser();

            entity.Slug = mailTemplateCreateDto.Slug;
            entity.Subject = mailTemplateCreateDto.Subject;
            entity.VisibilitySetting = entity.VisibilitySetting;
            entity.Html = entity.Html;
            entity.UserCreatedId = currentUser.Id;
            entity.UserCreated = currentUser.FirstName+" "+currentUser.LastName;
            entity.CompanyId = currentUser.CompanyId;
            entity.DateCreation = DateTime.UtcNow;
            if (mailTemplateCreateDto.MailAttachments != null
                && mailTemplateCreateDto.MailAttachments.Count != 0)
            {
                foreach (var mailAttachmentDto in mailTemplateCreateDto.MailAttachments)
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
            }

            var created = await _repository.CreateAsync(entity);

            return _mapper.Map<MailTemplateDto>(created);
        }
    }
}
