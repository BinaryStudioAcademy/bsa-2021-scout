using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Domain.Common;
using Domain.Interfaces.Abstractions;
using Application.Common.Models;

namespace Application.Common.Queries
{
    public class GetEntityByIdQuery<TDto> : IRequest<TDto>
        where TDto : Dto
    {
        public string Id { get; }

        public GetEntityByIdQuery(string id)
        {
            Id = id;
        }
    }

    public class GetEntitiesQueryHandler<TEntity, TDto> : IRequestHandler<GetEntityByIdQuery<TDto>, TDto>
        where TEntity : Entity
        where TDto : Dto
    {
        protected readonly IReadRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public GetEntitiesQueryHandler(IReadRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TDto> Handle(GetEntityByIdQuery<TDto> query, CancellationToken _)
        {
            TEntity result = await _repository.GetAsync(query.Id);

            return _mapper.Map<TDto>(result);
        }
    }
}
