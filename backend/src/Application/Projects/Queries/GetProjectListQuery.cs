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
    public class GetProjectListQuery : IRequest<IEnumerable<ProjectGetDto>>
    { }
    public class GetProjectListQueryHandler : IRequestHandler<GetProjectListQuery, IEnumerable<ProjectGetDto>>
    {
        protected readonly IReadRepository<Project> _repository;
        protected readonly IMapper _mapper;

        public GetProjectListQueryHandler(IReadRepository<Project> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectGetDto>> Handle(GetProjectListQuery query, CancellationToken _)
        {
            IEnumerable<Project> result = await _repository.GetEnumerableAsync();

            return _mapper.Map<IEnumerable<ProjectGetDto>>(result);
        }
    }
}
