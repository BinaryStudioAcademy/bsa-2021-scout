using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Domain.Common;
using Domain.Interfaces.Abstractions;

namespace Application.Common.Commands
{
    public class DeleteEntityCommand : IRequest
    {
        public string Id { get; }

        public DeleteEntityCommand(string id)
        {
            Id = id;
        }
    }

    public class DeleteEntityCommandHandler<TEntity> : IRequestHandler<DeleteEntityCommand>
        where TEntity : Entity
    {
        protected readonly IWriteRepository<TEntity> _repository;

        public DeleteEntityCommandHandler(IWriteRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteEntityCommand command, CancellationToken _)
        {
            await _repository.DeleteAsync(command.Id);

            return await Task.FromResult<Unit>(Unit.Value);
        }
    }
}
