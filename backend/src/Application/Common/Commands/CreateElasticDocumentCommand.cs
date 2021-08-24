using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Domain.Common;
using Domain.Interfaces.Abstractions;
using Application.Common.Models;

namespace Application.Common.Commands
{
    public class CreateElasticDocumentCommand<TDto> : IRequest<TDto>
        where TDto : Dto
    {
        public TDto Entity { get; }

        public CreateElasticDocumentCommand(TDto entity)
        {
            Entity = entity;
        }
    }

    public class CreateElasticDocumentCommandHandler<TDocument, TDto> : IRequestHandler<CreateElasticDocumentCommand<TDto>, TDto>
        where TDocument : Entity
        where TDto : Dto
    {
        protected readonly IElasticWriteRepository<TDocument> _repository;
        protected readonly IMapper _mapper;

        public CreateElasticDocumentCommandHandler(IElasticWriteRepository<TDocument> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TDto> Handle(CreateElasticDocumentCommand<TDto> command, CancellationToken _)
        {
            TDocument entity = _mapper.Map<TDocument>(command.Entity);
            Entity created = await _repository.CreateAsync(entity);

            return _mapper.Map<TDto>(created);
        }
    }
}
