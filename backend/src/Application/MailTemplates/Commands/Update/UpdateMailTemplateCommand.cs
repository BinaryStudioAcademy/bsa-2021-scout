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

        public UpdateMailTemplateCommandHandler(IWriteRepository<MailTemplate> writeRepository,
            IReadRepository<MailTemplate> readRepository,
            IMapper mapper,
            ICurrentUserContext currentUserContext)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
            _mapper = mapper;
            _currentUserContext = currentUserContext;
        }

        public async Task<MailTemplateDto> Handle(UpdateMailTemplateCommand command, CancellationToken cancellationToken)
        {
            var mailTemplate = await _readRepository.GetAsync(command.MailTemplateDto.Id);
            if(mailTemplate == null)
            {
                throw new NotFoundException(typeof(MailTemplate), command.MailTemplateDto.Id);
            }

            var updatedMailTemplate = _mapper.Map<MailTemplate>(command.MailTemplateDto);

            updatedMailTemplate.CompanyId = mailTemplate.CompanyId;
            updatedMailTemplate.DateCreation = mailTemplate.DateCreation;
            updatedMailTemplate.UserCreatedId = mailTemplate.UserCreatedId;

            var created = await _writeRepository.UpdateAsync(updatedMailTemplate);

            return _mapper.Map<MailTemplateDto>(created);
        }
    }
}
