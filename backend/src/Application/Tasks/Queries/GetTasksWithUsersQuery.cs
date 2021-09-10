using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Domain.Common;
using Domain.Interfaces.Abstractions;
using Domain.Entities;
using Domain.Interfaces.Read;
using Application.Common.Exceptions;
using Application.Tasks.Dtos;
using System.Collections.Generic;
using Application.ElasticEnities.Dtos;

namespace Application.Common.Queries
{
    public class GetTasksWithTeamMembersQuery : IRequest<List<TaskDto>>
    {
        public GetTasksWithTeamMembersQuery()
        {            
        }
    }

    public class GetTaskWithApplicantsQueryHandler : IRequestHandler<GetTasksWithTeamMembersQuery, List<TaskDto>>
    {
        protected readonly ITaskReadRepository _repository;
        protected readonly IMapper _mapper;
        protected readonly ISender _mediator;

        public GetTaskWithApplicantsQueryHandler(ITaskReadRepository repository, ISender mediator, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<List<TaskDto>> Handle(GetTasksWithTeamMembersQuery query, CancellationToken _)
        {
            var result = await _repository.GetTasksWithTeamMembersAsync();
            var res =_mapper.Map<List<TaskDto>>(result);
            return res;
        }
    }
}
