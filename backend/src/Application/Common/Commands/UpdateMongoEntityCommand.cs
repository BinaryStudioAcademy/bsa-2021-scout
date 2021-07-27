using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Domain.Common;
using Domain.Interfaces;
using Application.Common.Models;

namespace Application.Common.Commands
{
    public class UpdateMongoEntityCommand<TDto> : IRequest<TDto>
        where TDto : Dto
    {
        public TDto Entity { get; }

        public UpdateMongoEntityCommand(TDto entity)
        {
            Entity = entity;
        }
    }

    public class UpdateMongoEntityCommandHandler<TEntity, TDto> : IRequestHandler<UpdateMongoEntityCommand<TDto>, TDto>
        where TEntity : MongoEntity
        where TDto : Dto
    {
        protected readonly IMongoWriteRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public UpdateMongoEntityCommandHandler(IMongoWriteRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TDto> Handle(UpdateMongoEntityCommand<TDto> command, CancellationToken _)
        {
            TEntity entity = _mapper.Map<TEntity>(command.Entity);
            MongoEntity updated = await _repository.UpdateAsync(entity);

            return _mapper.Map<TDto>(updated);
        }
    }
}
