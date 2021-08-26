using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.UserFollowed.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using MediatR;

namespace Application.UserFollowed.Commands
{
    public class DeleteUserFollowedCommand: IRequest<Unit>
    {
        public string EntityId { get; set; }
        public EntityType EntityType { get; set; }

        public DeleteUserFollowedCommand(string entityId, EntityType entityType)
        {
            EntityId = entityId;
            EntityType = entityType;
        }
    }
    public class DeleteUserFollowedCommandHandler : IRequestHandler<DeleteUserFollowedCommand, Unit>
    {
        private readonly IWriteRepository<UserFollowedEntity> _writeRepo;
        private readonly IUserFollowedReadRepository _readRepo;
        private readonly IMapper _mapper;
        private readonly ICurrentUserContext _currentUserContext;
        public DeleteUserFollowedCommandHandler(IUserFollowedReadRepository readRepo, ICurrentUserContext currentUserContext,IMapper mapper, IWriteRepository<UserFollowedEntity> writeRepo)
        {
            _writeRepo = writeRepo;
            _readRepo = readRepo;
            _mapper = mapper;
            _currentUserContext = currentUserContext;
        }

        public async Task<Unit> Handle(DeleteUserFollowedCommand request, CancellationToken cancellationToken)
        {
           var follow = await _readRepo.GetForCurrentUserByTypeAndEntityId(request.EntityId, request.EntityType);
           await _writeRepo.DeleteAsync(follow.Id);
           return Unit.Value;
        }
    }
}