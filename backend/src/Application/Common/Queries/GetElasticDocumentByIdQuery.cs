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
    public class GetElasticDocumentByIdQuery<TDto> : IRequest<TDto>
        where TDto : Dto
    {
        public string Id { get; }

        public GetElasticDocumentByIdQuery(string id)
        {
            Id = id;
        }
    }

    public class GetElasticDocumentByIdQueryHandler<TDocument, TDto> : IRequestHandler<GetElasticDocumentByIdQuery<TDto>, TDto>
        where TDocument : Entity
        where TDto : Dto
    {
        protected readonly IElasticReadRepository<TDocument> _repository;
        protected readonly IMapper _mapper;

        public GetElasticDocumentByIdQueryHandler(IElasticReadRepository<TDocument> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TDto> Handle(GetElasticDocumentByIdQuery<TDto> query, CancellationToken _)
        {
            TDocument result = await _repository.GetAsync(query.Id);

            return _mapper.Map<TDto>(result);
        }
    }
}
