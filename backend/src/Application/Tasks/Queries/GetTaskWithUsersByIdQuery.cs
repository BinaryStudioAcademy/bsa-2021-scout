using Application.Common.Exceptions;
using Application.ElasticEnities.Dtos;
using Application.Tasks.Dtos;
using AutoMapper;
using Domain.Interfaces.Read;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Queries
{
    public class GetTaskWithTeamMembersByIdQuery : IRequest<TaskDto>
    {
        public string Id { get; }
        public GetTaskWithTeamMembersByIdQuery(string id)
        {
            Id = id;
        }
    }

    public class GetTaskWithApplicantsByIdQueryHandler : IRequestHandler<GetTaskWithTeamMembersByIdQuery, TaskDto>
    {
        protected readonly ITaskReadRepository _repository;
        protected readonly IMapper _mapper;
        protected readonly ISender _mediator;

        public GetTaskWithApplicantsByIdQueryHandler(ITaskReadRepository repository, ISender mediator, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
        }


        public async Task<TaskDto> Handle(GetTaskWithTeamMembersByIdQuery query, CancellationToken _)
        {
            var result = await _repository.GetTaskWithTeamMembersByIdAsync(query.Id);

            if (result == null) throw new NotFoundException(typeof(TaskDto), query.Id);


            return _mapper.Map<TaskDto>(result);
        }
    }
}
