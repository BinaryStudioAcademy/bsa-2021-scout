using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Domain.Common;
using Domain.Interfaces.Abstractions;
using Application.Common.Models;

namespace Application.Common.Commands
{
    public class CreateEntityCommand<TDto> : IRequest<TDto>
        where TDto : Dto
    {
        public TDto Entity { get; }

        public CreateEntityCommand(TDto entity)
        {
            Entity = entity;
        }
    }

    public class CreateEntityCommandHandler<TEntity, TDto> : IRequestHandler<CreateEntityCommand<TDto>, TDto>
        where TEntity : Entity
        where TDto : Dto
    {
        protected readonly IWriteRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public CreateEntityCommandHandler(IWriteRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TDto> Handle(CreateEntityCommand<TDto> command, CancellationToken _)
        {
            TEntity entity = _mapper.Map<TEntity>(command.Entity);
            Entity created = await _repository.CreateAsync(entity);

            return _mapper.Map<TDto>(created);
        }
    }
}
