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
using Application.MailAttachments.Commands;

namespace Application.MailTemplates.Commands
{
    public class DeleteMailTemplateCommand : IRequest<Unit>
    {

        public string Id;

        public DeleteMailTemplateCommand(string id)
        {
            Id = id;
        }
    }

    public class DeleteMailTemplateCommandHandler : IRequestHandler<DeleteMailTemplateCommand, Unit>
    {
        protected readonly IWriteRepository<MailTemplate> _writeRepository;
        protected readonly IReadRepository<MailTemplate> _readRepository;
        protected readonly ISender _mediator;

        public DeleteMailTemplateCommandHandler(IWriteRepository<MailTemplate> writeRepository,
            IReadRepository<MailTemplate> readRepository,
            ISender mediator)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(DeleteMailTemplateCommand command, CancellationToken cancellationToken)
        {
            var mailTemplate = await _readRepository.GetAsync(command.Id);
            if (mailTemplate == null)
            {
                throw new NotFoundException(typeof(MailTemplate), command.Id);
            }
            if (mailTemplate.MailAttachments != null
                && mailTemplate.MailAttachments.Count != 0)
            {
                foreach (var mailAttachment in mailTemplate.MailAttachments)
                {
                    var deleteMailAttachmentFileCommand = new DeleteMailAttachmentFileCommand(mailAttachment.Key);
                    await _mediator.Send(deleteMailAttachmentFileCommand);
                }
            }
            await _writeRepository.DeleteAsync(command.Id);

            return Unit.Value;
        }
    }
}
