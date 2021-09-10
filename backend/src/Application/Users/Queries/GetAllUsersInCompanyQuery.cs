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
    public class GetAllUsersInCompanyQuery : IRequest<IEnumerable<UserDto>>
    {

    }
    public class GetAllUsersInCompanyQueryHandler : IRequestHandler<GetAllUsersInCompanyQuery, IEnumerable<UserDto>>
    {
        protected readonly ICurrentUserContext _currentUserContext;
        protected readonly IUserReadRepository _userReadRepository;
        protected readonly IMapper _mapper;

        public GetAllUsersInCompanyQueryHandler(ICurrentUserContext currentUserContext, IUserReadRepository userReadRepository,
                                            IMapper mapper)
        {
            _currentUserContext = currentUserContext;
            _userReadRepository = userReadRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetAllUsersInCompanyQuery query, CancellationToken _)
        {
            var currentUser = await _currentUserContext.GetCurrentUser();

            var users = await _userReadRepository.GetUsersByCompanyIdAsync(currentUser.CompanyId);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}
