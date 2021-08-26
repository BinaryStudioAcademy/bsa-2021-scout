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

        public DeleteMailTemplateCommandHandler(IWriteRepository<MailTemplate> writeRepository,
            IReadRepository<MailTemplate> readRepository)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
        }

        public async Task<Unit> Handle(DeleteMailTemplateCommand command, CancellationToken cancellationToken)
        {
            var mailTempale = await _readRepository.GetAsync(command.Id);
            if(mailTempale == null)
            {
                throw new NotFoundException(typeof(MailTemplate), command.Id);
            }

            await _writeRepository.DeleteAsync(command.Id);

            return Unit.Value;
        }
    }
}
