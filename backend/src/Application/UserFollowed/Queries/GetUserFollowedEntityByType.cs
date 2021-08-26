using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.UserFollowed.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using MediatR;

namespace Application.UserFollowed.Queries
{
    public class GetUserFollowedEntityByType: IRequest<IEnumerable<UserFollowedDto>>
    {
        public EntityType EntityType { get; }

        public GetUserFollowedEntityByType(EntityType type)
        {
            EntityType = type;
        }
    }
    public class GetUserFollowedEntityByTypeHandler  : IRequestHandler<GetUserFollowedEntityByType, IEnumerable<UserFollowedDto>>
    {
        private readonly IUserFollowedReadRepository _repo;
        private readonly IMapper _mapper;
        public GetUserFollowedEntityByTypeHandler(IMapper mapper, IUserFollowedReadRepository repo)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UserFollowedDto>> Handle(GetUserFollowedEntityByType request, CancellationToken cancellationToken)
        {
            var followedList = await _repo.GetEnumerableForCurrentUserByType(request.EntityType);
            return _mapper.Map<IEnumerable<UserFollowedDto>>(followedList);
        }
    }
}