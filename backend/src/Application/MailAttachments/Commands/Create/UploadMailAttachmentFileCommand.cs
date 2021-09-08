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
using Domain.Interfaces.Write;
using System.IO;

namespace Application.MailTemplates.Commands
{
    public class UploadMailAttachmentFileCommand : IRequest<string>
    {
        public string Id { get; }
        public string MailTemplateId { get; }
        public MailAttachmentCreateDto MailAttachmentDto { get; }
        public UploadMailAttachmentFileCommand(string id, string mailTemplateId, MailAttachmentCreateDto mailAttachmentDto)
        {
            Id = id;
            MailTemplateId = mailTemplateId;
            MailAttachmentDto = mailAttachmentDto;
        }
    }

    public class UploadMailAttachmentFileCommandHandler : IRequestHandler<UploadMailAttachmentFileCommand, string> 
    {
        protected readonly IMailAttachmentFileWriteRepository _mailAttachmentFileWriteRepository;

        public UploadMailAttachmentFileCommandHandler(IMailAttachmentFileWriteRepository mailAttachmentFileWriteRepository)
        {
            _mailAttachmentFileWriteRepository = mailAttachmentFileWriteRepository;
        }

        public async Task<string> Handle(UploadMailAttachmentFileCommand command, CancellationToken cancellationToken)
        {
            Stream stream = command.MailAttachmentDto.File.OpenReadStream();
            MemoryStream memory = new MemoryStream();
            stream.CopyTo(memory);
            var key = $"MailTemplate/{command.MailTemplateId}/{command.Id}/{command.MailAttachmentDto.Name}";
            await _mailAttachmentFileWriteRepository.UploadAsync(
                key,
                stream);

            return key;
        }
    }
}
