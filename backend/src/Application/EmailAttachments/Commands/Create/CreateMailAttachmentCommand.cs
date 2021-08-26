using Application.Common.Commands;
using Application.MailAttachments.Dtos;
using Application.Interfaces;
using Application.MailTemplates.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;

namespace Application.MailTemplates.Commands
{
    public class CreateMailAttachmentCommand : IRequest<MailAttachmentDto>
    {
        public MailAttachmentCreateDto MailAttachmentDto { get; }
        public CreateMailAttachmentCommand(MailAttachmentCreateDto mailAttachmentDto)
        {
            MailAttachmentDto = mailAttachmentDto;
        }
    }

    public class CreateMailAttachmentCommandHandler : IRequestHandler<CreateMailAttachmentCommand, MailAttachmentDto> 
    {
        protected readonly IWriteRepository<MailAttachment> _mailAttachmentWriteRepository;
        protected readonly IReadRepository<MailTemplate> _mailTemplateReadRepository;

        protected readonly IMapper _mapper;
        protected readonly ICurrentUserContext _currentUserContext;

        public CreateMailAttachmentCommandHandler(IWriteRepository<MailAttachment> mailAttachmentWriteRepository,
            IReadRepository<MailTemplate> mailTemplateReadRepository,
            IMapper mapper, 
            ICurrentUserContext currentUserContext)
        {
            _mailAttachmentWriteRepository = mailAttachmentWriteRepository;
            _mailTemplateReadRepository = mailTemplateReadRepository;
            _mapper = mapper;
            _currentUserContext = currentUserContext;
        }

        public async Task<MailAttachmentDto> Handle(CreateMailAttachmentCommand command, CancellationToken cancellationToken)
        {
            var mailTemplate = _mailTemplateReadRepository.GetAsync(command.MailAttachmentDto.MailTemplateId);
            if (mailTemplate == null)
            {
                throw new NotFoundException(typeof(MailTemplate), command.MailAttachmentDto.MailTemplateId);
            }

            var entity = _mapper.Map<MailAttachment>(command.MailAttachmentDto);

            var created = await _mailAttachmentWriteRepository.CreateAsync(entity);

            return _mapper.Map<MailAttachmentDto>(created);
        }
    }
}
