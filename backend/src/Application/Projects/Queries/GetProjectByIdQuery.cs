using Application.Common.Queries;
using Application.Projects.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Projects.Queries
{
    public class GetProjectByIdQuery: IRequest<ProjectGetDto>
    {
        public string Id { get; }

        public GetProjectByIdQuery(string id)
        {
            Id = id;
        }
    }

    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectGetDto>
    {
        protected readonly IReadRepository<Project> _repository;
        protected readonly IMapper _mapper;

        public GetProjectByIdQueryHandler(IReadRepository<Project> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProjectGetDto> Handle(GetProjectByIdQuery query, CancellationToken _)
        {
            Project result = await _repository.GetAsync(query.Id);

            return _mapper.Map<ProjectGetDto>(result);
        }
    }
}
