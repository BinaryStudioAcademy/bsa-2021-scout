using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Domain.Common;
using Domain.Interfaces.Abstractions;
using Application.Common.Models;
using System.Collections.Generic;

namespace Application.Common.Commands
{
    public class CreateBulkElasticDocumentCommand<TDto> : IRequest
    {
        public IEnumerable<TDto> Bulk { get; }

        public CreateBulkElasticDocumentCommand(IEnumerable<TDto> bulk)
        {
            Bulk = bulk;
        }
    }

    public class CreateBulkElasticDocumentCommandHandler<TDocument, TDto> : IRequestHandler<CreateBulkElasticDocumentCommand<TDto>>
        where TDocument : Entity
        where TDto : Dto
    {
        protected readonly IElasticWriteRepository<TDocument> _repository;
        protected readonly IMapper _mapper;

        public CreateBulkElasticDocumentCommandHandler(IElasticWriteRepository<TDocument> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(CreateBulkElasticDocumentCommand<TDto> command, CancellationToken _)
        {
            IEnumerable<TDocument> entity = _mapper.Map<IEnumerable<TDocument>>(command.Bulk);
            await _repository.InsertBulkAsync(entity);

            return await Task.FromResult<Unit>(Unit.Value);
        }
    }
}
