using Application.Tasks.Dtos;
using Application.Common.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;

namespace Application.Tasks.Queries.GetTaskByIdQueryFull
{
    public class GetTaskByIdQueryFullHandler : GetEntitiesQueryHandler<ToDoTask, TaskDto>
    {
        public GetTaskByIdQueryFullHandler(IReadRepository<ToDoTask> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
