using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using MongoDB.Bson;
using Domain.Common;
using Domain.Interfaces;
using Application.Common.Models;

namespace Application.Common.Queries
{
    public class GetMongoEntityByIdQuery<TDto> : IRequest<TDto>
        where TDto : Dto
    {
        public ObjectId Id { get; }

        public GetMongoEntityByIdQuery(ObjectId id)
        {
            Id = id;
        }
    }

    public class GetMongoEntityByIdQueryHandler<TEntity, TDto> : IRequestHandler<GetMongoEntityByIdQuery<TDto>, TDto>
        where TEntity : MongoEntity
        where TDto : Dto
    {
        protected readonly IMongoReadRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public GetMongoEntityByIdQueryHandler(IMongoReadRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TDto> Handle(GetMongoEntityByIdQuery<TDto> query, CancellationToken _)
        {
            TEntity result = await _repository.GetAsync(query.Id);

            return _mapper.Map<TDto>(result);
        }
    }
}
