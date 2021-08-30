using Application.Tasks.Dtos;
using Application.Common.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;

namespace Application.Tasks.Queries.GetTaskById
{
    public class GetTaskByIdQueryHandler : GetEntitiesQueryHandler<ToDoTask, TaskDto>
    {
        public GetTaskByIdQueryHandler(IReadRepository<ToDoTask> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
