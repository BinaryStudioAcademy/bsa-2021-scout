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
using Domain.Interfaces.Write;
using System.IO;

namespace Application.MailAttachments.Commands
{
    public class DeleteMailAttachmentFileCommand : IRequest<Unit>
    {

        public string Key;

        public DeleteMailAttachmentFileCommand(string key)
        {
            Key = key;
        }
    }

    public class DeleteMailAttachmentFileCommandHandler : IRequestHandler<DeleteMailAttachmentFileCommand, Unit>
    {
        protected readonly IMailAttachmentFileWriteRepository _mailAttachmentFileWriteRepository;

        public DeleteMailAttachmentFileCommandHandler(IMailAttachmentFileWriteRepository mailAttachmentFileWriteRepository)
        {
            _mailAttachmentFileWriteRepository = mailAttachmentFileWriteRepository;
        }

        public async Task<Unit> Handle(DeleteMailAttachmentFileCommand command, CancellationToken cancellationToken)
        {
            var path = Path.GetFullPath(command.Key);
            var name = Path.GetFileName(command.Key);
            await _mailAttachmentFileWriteRepository.DeleteAsync(path, name);

            return Unit.Value;
        }
    }
}
