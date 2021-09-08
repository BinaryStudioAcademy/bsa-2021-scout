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
    public class GetArchivedVacanciesQuery : IRequest<IEnumerable<ArchivedVacancyDto>>
    {

    }
    public class GetArchivedVacanciesQueryHandler : IRequestHandler<GetArchivedVacanciesQuery, IEnumerable<ArchivedVacancyDto>>
    {
        protected readonly ICurrentUserContext _currentUserContext;
        protected readonly IArchivedEntityReadRepository _archivedEntityReadRepository;
        protected readonly IMapper _mapper;

        public GetArchivedVacanciesQueryHandler(ICurrentUserContext currentUserContext, IArchivedEntityReadRepository archivedEntityReadRepository,
                                                IMapper mapper)
        {
            _currentUserContext = currentUserContext;
            _archivedEntityReadRepository = archivedEntityReadRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArchivedVacancyDto>> Handle(GetArchivedVacanciesQuery query, CancellationToken _)
        {
            var companyId = (await _currentUserContext.GetCurrentUser()).CompanyId;

            var archivedVacancies = await _archivedEntityReadRepository.GetArchivedVacanciesAsync(companyId);

            return _mapper.Map<IEnumerable<ArchivedVacancyDto>>(archivedVacancies);
        }
    }
}
