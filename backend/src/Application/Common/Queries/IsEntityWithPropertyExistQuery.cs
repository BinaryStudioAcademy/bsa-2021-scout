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
    public class IsEntityWithPropertyExistQuery : IRequest<bool>
    {
        public string Property { get; }

        public string PropertyValue { get; }

        public IsEntityWithPropertyExistQuery(string property, string propertyValue)
        {
            Property = property;

            PropertyValue = propertyValue;
        }
    }

    public class IsEntityWithPropertyExistQueryHandler<TEntity> : IRequestHandler<IsEntityWithPropertyExistQuery, bool>
        where TEntity : Entity
    {
        protected readonly IReadRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public IsEntityWithPropertyExistQueryHandler(IReadRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(IsEntityWithPropertyExistQuery query, CancellationToken _)
        {
            TEntity result = await _repository.GetByPropertyAsync(query.Property, query.PropertyValue);
            if(result != null)
            {
                return true;
            }
            return false;
        }
    }
}
