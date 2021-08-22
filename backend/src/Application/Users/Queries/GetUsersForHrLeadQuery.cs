using Application.Auth.Exceptions;
using Application.Interfaces;
using Application.Users.Dtos;
using AutoMapper;
using Domain.Interfaces.Read;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries
{
    public class GetUsersForHrLeadQuery: IRequest<IEnumerable<UserDto>>
    {

    }
    public class GetUserForHrLeadQueryHandler : IRequestHandler<GetUsersForHrLeadQuery, IEnumerable<UserDto>>
    {
        protected readonly ICurrentUserContext _currentUserContext;
        protected readonly IUserReadRepository _userReadRepository;
        protected readonly IMapper _mapper;

        public GetUserForHrLeadQueryHandler(ICurrentUserContext currentUserContext, IUserReadRepository userReadRepository,
                                            IMapper mapper)
        {
            _currentUserContext = currentUserContext;
            _userReadRepository = userReadRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetUsersForHrLeadQuery query, CancellationToken _)
        {
            var hrLead = await _currentUserContext.GetCurrentUser();

            if (hrLead is null)
            {
                throw new InvalidTokenException("access");
            }

            var users = await _userReadRepository.GetUsersByCompanyIdAsync(hrLead.CompanyId);
            users = users.Where(u => u.Id != hrLead.Id);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}
