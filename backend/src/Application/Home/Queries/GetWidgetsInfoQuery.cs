using Application.Home.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces.Read;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Home.Queries
{
    public class GetWidgetsDataQuery : IRequest<WidgetsDataDto>
    {

    }
    public class GetWidgetsDataQueryHandler : IRequestHandler<GetWidgetsDataQuery, WidgetsDataDto>
    {
        protected readonly ICurrentUserContext _currentUserContext;
        protected readonly IHomeDataReadRepository _homePageInfoReadRepository;
        protected readonly IMapper _mapper;

        public GetWidgetsDataQueryHandler(ICurrentUserContext currentUserContext, IHomeDataReadRepository homePageInfoReadRepository,
                                          IMapper mapper)
        {
            _currentUserContext = currentUserContext;
            _homePageInfoReadRepository = homePageInfoReadRepository;
            _mapper = mapper;
        }

        public async Task<WidgetsDataDto> Handle(GetWidgetsDataQuery query, CancellationToken _)
        {
            var hrLead = await _currentUserContext.GetCurrentUser();

            var widgetsData = await _homePageInfoReadRepository.GetWidgetsDataAsync(hrLead.CompanyId);
            return _mapper.Map<WidgetsDataDto>(widgetsData);
        }
    }
}