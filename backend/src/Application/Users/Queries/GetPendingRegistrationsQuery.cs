using Application.Interfaces;
using Application.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries
{
    public class GetPendingRegistrationsQuery : IRequest<IEnumerable<RegisterPermissionShortDto>>
    {

    }

    public class GetPendingRegistrationsQueryHandler : IRequestHandler<GetPendingRegistrationsQuery, IEnumerable<RegisterPermissionShortDto>>
    {
        private readonly IReadRepository<RegisterPermission> _registerPermissionReadRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserContext _currentUserContext;
        public GetPendingRegistrationsQueryHandler(IReadRepository<RegisterPermission> registerPermissionReadRepository, IMapper mapper, ICurrentUserContext currentUserContext)
        {
            _registerPermissionReadRepository = registerPermissionReadRepository;
            _mapper = mapper;
            _currentUserContext = currentUserContext;
        }
        public async Task<IEnumerable<RegisterPermissionShortDto>> Handle(GetPendingRegistrationsQuery query, CancellationToken _)
        {
            var companyId = (await _currentUserContext.GetCurrentUser()).CompanyId;

            var registerPermissions = await _registerPermissionReadRepository.GetEnumerableAsync();

            return _mapper.Map<IEnumerable<RegisterPermissionShortDto>>(registerPermissions.Where(r => r.CompanyId == companyId));
        }
    }
}
