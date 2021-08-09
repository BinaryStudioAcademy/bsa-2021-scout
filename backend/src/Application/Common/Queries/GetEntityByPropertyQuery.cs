using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Domain.Common;
using Domain.Interfaces.Abstractions;
using Application.Common.Models;
using Application.Users.Dtos;

namespace Application.Common.Queries
{
    public class GetEntityByPropertyQuery<TDto> : IRequest<TDto>
        where TDto : Dto
    {
        public string Property { get; }

        public string PropertyValue { get; }

        public GetEntityByPropertyQuery(string property, string propertyValue)
        {
            Property = property;

            PropertyValue = propertyValue;
        }
    }

    public class GetEntityByPropertyQueryHandler<TEntity, TDto> : IRequestHandler<GetEntityByPropertyQuery<TDto>, TDto>
        where TEntity : Entity
        where TDto : Dto
    {
        protected readonly IReadRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public GetEntityByPropertyQueryHandler(IReadRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TDto> Handle(GetEntityByPropertyQuery<TDto> query, CancellationToken _)
        {
            TEntity result = await _repository.GetByPropertyAsync(query.Property, query.PropertyValue);
            return  _mapper.Map<TDto>(result);
        }
    }
}
