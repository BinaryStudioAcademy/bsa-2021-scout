using AutoMapper;
using Domain.Interfaces.Read;
using MediatR;
using System.Collections.Generic;
using Application.Arhive.Dtos;
using System.Threading;
using Application.Interfaces;
using System.Threading.Tasks;

namespace Application.Pools.Queries.GetPoolByIdQueryFull
{
    public class GetArchivedProjectsQuery : IRequest<IEnumerable<ArchivedProjectDto>>
    {

    }
    public class GetArchivedProjectsQueryHandler : IRequestHandler<GetArchivedProjectsQuery, IEnumerable<ArchivedProjectDto>>
    {
        protected readonly ICurrentUserContext _currentUserContext;
        protected readonly IArchivedEntityReadRepository _archivedEntityReadRepository;
        protected readonly IMapper _mapper;

        public GetArchivedProjectsQueryHandler(ICurrentUserContext currentUserContext, IArchivedEntityReadRepository archivedEntityReadRepository,
                                                IMapper mapper)
        {
            _currentUserContext = currentUserContext;
            _archivedEntityReadRepository = archivedEntityReadRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArchivedProjectDto>> Handle(GetArchivedProjectsQuery query, CancellationToken _)
        {
            var companyId = (await _currentUserContext.GetCurrentUser()).CompanyId;

            var archivedProjects = await _archivedEntityReadRepository.GetArchivedProjectsAsync(companyId);

            return _mapper.Map<IEnumerable<ArchivedProjectDto>>(archivedProjects);
        }
    }
}
