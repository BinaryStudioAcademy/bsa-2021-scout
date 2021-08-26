using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.UserFollowed.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;

namespace Application.UserFollowed.Commands
{
    public class CreateUserFollowedCommand: IRequest<UserFollowedDto>
    {
        public CreateUserFollowedDto FollowedDto { get; set; }

        public CreateUserFollowedCommand(CreateUserFollowedDto dto)
        {
            FollowedDto = dto;
        }
    }
    public class CreateUserFollowedCommandHandlers : IRequestHandler<CreateUserFollowedCommand, UserFollowedDto>
    {
        private readonly IWriteRepository<UserFollowedEntity> _writeRepo;
        private readonly IMapper _mapper;
        private readonly ICurrentUserContext _currentUserContext;
        public CreateUserFollowedCommandHandlers(ICurrentUserContext currentUserContext,IMapper mapper, IWriteRepository<UserFollowedEntity> writeRepo)
        {
            _writeRepo = writeRepo;
            _mapper = mapper;
            _currentUserContext = currentUserContext;
        }
        public async Task<UserFollowedDto> Handle(CreateUserFollowedCommand request, CancellationToken cancellationToken)
        {
           var entity = _mapper.Map<UserFollowedEntity>(request.FollowedDto);
           entity.UserId = (await _currentUserContext.GetCurrentUser()).Id;
           var createDto = _mapper.Map<UserFollowedDto>(await _writeRepo.CreateAsync(entity));
           return createDto;
        }
    }
}