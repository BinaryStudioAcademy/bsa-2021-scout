using Domain.Entities;
using Application.Common.Commands;
using Domain.Interfaces.Abstractions;
using Application.MailAttachments.Dtos;
using AutoMapper;
using MediatR;
using Application.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using System;
using Application.Common.Exceptions;
using Domain.Interfaces.Read;

namespace Application.MailAttachments.Commands
{
    public class UpdateMailAttachmentCommand : IRequest<MailAttachmentDto>
    {
        public MailAttachmentUpdateDto MailAttachmentDto { get; }
        public UpdateMailAttachmentCommand(MailAttachmentUpdateDto mailAttachmentDto)
        {
            MailAttachmentDto = mailAttachmentDto;
        }
    }

    public class UpdateMailAttachmentCommandHandler : IRequestHandler<UpdateMailAttachmentCommand, MailAttachmentDto>
    {
        protected readonly IWriteRepository<MailAttachment> _writeRepository;
        protected readonly IMailAttachmentReadRepository _readRepository;
        protected readonly IReadRepository<MailTemplate> _mailTemplateReadRepository;

        protected readonly IMapper _mapper;
        protected readonly ICurrentUserContext _currentUserContext;

        public UpdateMailAttachmentCommandHandler(IWriteRepository<MailAttachment> writeRepository,
            IMailAttachmentReadRepository readRepository,
            IMapper mapper,
            ICurrentUserContext currentUserContext)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
            _mapper = mapper;
            _currentUserContext = currentUserContext;
        }

        public async Task<MailAttachmentDto> Handle(UpdateMailAttachmentCommand command, CancellationToken cancellationToken)
        {
            var mailTemplate = _mailTemplateReadRepository.GetAsync(command.MailAttachmentDto.MailTemplateId);
            if (mailTemplate == null)
            {
                throw new NotFoundException(typeof(MailTemplate), command.MailAttachmentDto.MailTemplateId);
            }

            var mailAttachment = await _readRepository.GetAsync(command.MailAttachmentDto.Id);
            if(mailAttachment == null)
            {
                throw new NotFoundException(typeof(MailAttachment), command.MailAttachmentDto.Id);
            }

            var updatedMailAttachment = _mapper.Map<MailAttachment>(command.MailAttachmentDto);

            var created = await _writeRepository.UpdateAsync(updatedMailAttachment);

            return _mapper.Map<MailAttachmentDto>(created);
        }
    }
}
