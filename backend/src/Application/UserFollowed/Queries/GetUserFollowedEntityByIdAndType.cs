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
    public class GetUserFollowedEntityByIdAndType: IRequest<UserFollowedDto>
    {
        public string DtoId { get; }
        public EntityType EntityType { get; }

        public GetUserFollowedEntityByIdAndType(string id, EntityType type)
        {
            DtoId = id;
            EntityType = type;
        }
    }
    public class GetUserFollowedEntityByIdAndTypeHandler : IRequestHandler<GetUserFollowedEntityByIdAndType, UserFollowedDto>
    {
        private readonly IUserFollowedReadRepository _repo;
        private readonly IMapper _mapper;
        public GetUserFollowedEntityByIdAndTypeHandler(IMapper mapper, IUserFollowedReadRepository repo)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<UserFollowedDto> Handle(GetUserFollowedEntityByIdAndType request, CancellationToken cancellationToken)
        {
            var followed = await _repo.GetForCurrentUserByTypeAndEntityId(request.DtoId, request.EntityType);
            return _mapper.Map<UserFollowedDto>(followed);
        }
    }
}