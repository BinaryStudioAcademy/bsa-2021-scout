using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Domain.Common;
using Domain.Interfaces.Abstractions;
using Application.Common.Models;

namespace Application.Common.Commands
{
    public class UpdateEntityCommand<TDto> : IRequest<TDto>
        where TDto : Dto
    {
        public TDto Entity { get; }

        public UpdateEntityCommand(TDto entity)
        {
            Entity = entity;
        }
    }

    public class UpdateEntityCommandHandler<TEntity, TDto> : IRequestHandler<UpdateEntityCommand<TDto>, TDto>
        where TEntity : Entity
        where TDto : Dto
    {
        protected readonly IWriteRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public UpdateEntityCommandHandler(IWriteRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TDto> Handle(UpdateEntityCommand<TDto> command, CancellationToken _)
        {
            TEntity entity = _mapper.Map<TEntity>(command.Entity);
            Entity updated = await _repository.UpdateAsync(entity);

            return _mapper.Map<TDto>(updated);
        }
    }
}
