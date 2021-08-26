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

namespace Application.MailAttachments.Commands
{
    public class DeleteMailAttachmentCommand : IRequest<Unit>
    {

        public string Id;

        public DeleteMailAttachmentCommand(string id)
        {
            Id = id;
        }
    }

    public class DeleteMailAttachmentCommandHandler : IRequestHandler<DeleteMailAttachmentCommand, Unit>
    {
        protected readonly IWriteRepository<MailAttachment> _writeRepository;
        protected readonly IReadRepository<MailAttachment> _readRepository;

        public DeleteMailAttachmentCommandHandler(IWriteRepository<MailAttachment> writeRepository,
            IReadRepository<MailAttachment> readRepository)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
        }

        public async Task<Unit> Handle(DeleteMailAttachmentCommand command, CancellationToken cancellationToken)
        {
            var mailAttachment = await _readRepository.GetAsync(command.Id);
            if(mailAttachment == null)
            {
                throw new NotFoundException(typeof(MailAttachment), command.Id);
            }

            await _writeRepository.DeleteAsync(command.Id);

            return Unit.Value;
        }
    }
}
