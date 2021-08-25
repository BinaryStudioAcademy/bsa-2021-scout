using Application.Common.Commands;
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

        public CreateMailTemplateCommandHandler(IWriteRepository<MailTemplate> repository, 
            IMapper mapper, 
            ICurrentUserContext currentUserContext)
        {
            _repository = repository;
            _mapper = mapper;
            _currentUserContext = currentUserContext;
        }

        public async Task<MailTemplateDto> Handle(CreateMailTemplateCommand command, CancellationToken cancellationToken)
        {

            var entity = _mapper.Map<MailTemplate>(command.MailTemplateDto);

            var currentUser = await _currentUserContext.GetCurrentUser();

            entity.UserCreatedId = currentUser.Id;
            entity.CompanyId = currentUser.CompanyId;
            entity.DateCreation = DateTime.UtcNow;

            var created = await _repository.CreateAsync(entity);

            return _mapper.Map<MailTemplateDto>(created);
        }
    }
}
