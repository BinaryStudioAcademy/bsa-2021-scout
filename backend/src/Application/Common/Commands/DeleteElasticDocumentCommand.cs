using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Domain.Common;
using Domain.Interfaces.Abstractions;
using Application.Common.Models;

namespace Application.Common.Commands
{
    public class DeleteElasticDocumentCommand : IRequest
    {
        public string Id { get; }

        public DeleteElasticDocumentCommand(string id)
        {
            Id = id;
        }
    }

    public class DeleteElasticDocumentCommandHandler<TDocument, TDto> : IRequestHandler<DeleteElasticDocumentCommand>
        where TDocument : Entity
        where TDto : Dto
    {
        protected readonly IElasticWriteRepository<TDocument> _repository;

        public DeleteElasticDocumentCommandHandler(IElasticWriteRepository<TDocument> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteElasticDocumentCommand command, CancellationToken _)
        {
            await _repository.DeleteAsync(command.Id);

            return await Task.FromResult<Unit>(Unit.Value);
        }
    }
}
