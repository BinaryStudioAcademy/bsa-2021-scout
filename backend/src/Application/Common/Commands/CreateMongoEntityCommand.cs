using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Domain.Common;
using Domain.Interfaces;
using Application.Common.Models;

namespace Application.Common.Commands
{
    public class CreateMongoEntityCommand<TDto> : IRequest<TDto>
        where TDto : MongoDto
    {
        public TDto Entity { get; }

        public CreateMongoEntityCommand(TDto entity)
        {
            Entity = entity;
        }
    }

    public class CreateMongoEntityCommandHandler<TEntity, TDto> : IRequestHandler<CreateMongoEntityCommand<TDto>, TDto>
        where TEntity : MongoEntity
        where TDto : MongoDto
    {
        protected readonly IMongoWriteRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public CreateMongoEntityCommandHandler(IMongoWriteRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TDto> Handle(CreateMongoEntityCommand<TDto> command, CancellationToken _)
        {
            TEntity entity = _mapper.Map<TEntity>(command.Entity);
            MongoEntity created = await _repository.CreateAsync(entity);

            return _mapper.Map<TDto>(created);
        }
    }
}
