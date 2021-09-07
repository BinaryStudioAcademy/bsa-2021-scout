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
    public class GetTasksWithTeamMembersByUserQuery : IRequest<List<TaskDto>>
    {
        public string UserId { get; set; }
        public GetTasksWithTeamMembersByUserQuery(string userId)
        {
            UserId = userId;
        }
    }

    public class GetTasksWithUsersQueryByUserHandler : IRequestHandler<GetTasksWithTeamMembersByUserQuery, List<TaskDto>>
    {
        protected readonly ITaskReadRepository _repository;
        protected readonly IMapper _mapper;
        protected readonly ISender _mediator;

        public GetTasksWithUsersQueryByUserHandler(ITaskReadRepository repository, ISender mediator, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<List<TaskDto>> Handle(GetTasksWithTeamMembersByUserQuery query, CancellationToken _)
        {
            var result = await _repository.GetTasksWithTeamMembersByUserAsync(query.UserId);

            return _mapper.Map<List<TaskDto>>(result);
        }
    }
}
