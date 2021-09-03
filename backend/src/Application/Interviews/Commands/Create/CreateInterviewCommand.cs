using Application.Common.Commands;
using Application.ElasticEnities.Dtos;
using Application.Interfaces;
using Application.Interviews.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interviews.Commands.Create
{
    public class CreateInterviewCommand : IRequest<InterviewDto>
    {
        public CreateInterviewDto Interview { get; }

        public CreateInterviewCommand(CreateInterviewDto project)
        {
            Interview = project;
        }
    }

    public class CreateInterviewCommandHandler : IRequestHandler<CreateInterviewCommand, InterviewDto>
    {
        protected readonly IWriteRepository<Interview> _repository;
        protected readonly ICurrentUserContext _currentUserContext;
        protected readonly IMapper _mapper;

        public CreateInterviewCommandHandler(IWriteRepository<Interview> repository, 
            ICurrentUserContext currentUserContext, IMapper mapper)
        {
            _repository = repository;
            _currentUserContext = currentUserContext;
            _mapper = mapper;
        }

        public async Task<InterviewDto> Handle(CreateInterviewCommand command, CancellationToken _)
        {
            Interview entity = _mapper.Map<Interview>(command.Interview);

            entity.Id = Guid.NewGuid().ToString();
            entity.CreatedDate = DateTime.Now;
            entity.CompanyId = (await _currentUserContext.GetCurrentUser()).CompanyId;
            
            var created = await _repository.CreateAsync(entity);
            return _mapper.Map<InterviewDto>(created);
        }
    }
}
