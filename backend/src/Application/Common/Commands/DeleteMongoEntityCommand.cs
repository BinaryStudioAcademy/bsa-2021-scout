using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Bson;
using Domain.Common;
using Domain.Interfaces;

namespace Application.Common.Commands
{
    public class DeleteMongoEntityCommand : IRequest
    {
        public ObjectId Id { get; }

        public DeleteMongoEntityCommand(ObjectId id)
        {
            Id = id;
        }
    }

    public class DeleteMongoEntityCommandHandler<TEntity> : IRequestHandler<DeleteMongoEntityCommand>
        where TEntity : MongoEntity
    {
        protected readonly IMongoWriteRepository<TEntity> _repository;

        public DeleteMongoEntityCommandHandler(IMongoWriteRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteMongoEntityCommand command, CancellationToken _)
        {
            await _repository.DeleteAsync(command.Id);

            return await Task.FromResult<Unit>(Unit.Value);
        }
    }
}
