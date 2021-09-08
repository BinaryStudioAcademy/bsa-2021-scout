using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Domain.Common;
using Domain.Interfaces.Abstractions;
using Application.Common.Models;

namespace Application.Common.Commands
{
    public class UpdateElasticDocumentCommand<TDto> : IRequest<TDto>
        where TDto : Dto
    {
        public TDto Entity { get; }

        public UpdateElasticDocumentCommand(TDto entity)
        {
            Entity = entity;
        }
    }

    public class UpdateElasticDocumentCommandHandler<TDocument, TDto> : IRequestHandler<UpdateElasticDocumentCommand<TDto>, TDto>
        where TDocument : Entity
        where TDto : Dto
    {
        protected readonly IElasticWriteRepository<TDocument> _repository;
        protected readonly IMapper _mapper;

        public UpdateElasticDocumentCommandHandler(IElasticWriteRepository<TDocument> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TDto> Handle(UpdateElasticDocumentCommand<TDto> command, CancellationToken _)
        {
            TDocument entity = _mapper.Map<TDocument>(command.Entity);
            Entity updated = await _repository.UpdateAsync(entity);

            return _mapper.Map<TDto>(updated);
        }
    }
}
