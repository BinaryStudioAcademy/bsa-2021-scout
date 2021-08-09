using Application.Common.Commands;
using Application.Common.Exceptions;
using Application.Projects.Dtos;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Projects.CommandQuery.Delete
{
    public class DeleteProjectCommand : IRequest
    {
        public string Id { get; }

        public DeleteProjectCommand(string id)
        {
            Id = id;
        }
    }
    public class DeleteEntityCommandHandler : IRequestHandler<DeleteProjectCommand>
    {
        protected readonly IWriteRepository<Project> _writeRepository;
        protected readonly IReadRepository<Project> _readRepository;
        public DeleteEntityCommandHandler(IWriteRepository<Project> writeRepository,
            IReadRepository<Project> readRepository)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
        }

        public async Task<Unit> Handle(DeleteProjectCommand command, CancellationToken _)
        {
            var project = await _readRepository.GetAsync(command.Id);

            project.IsDeleted = true;
            await _writeRepository.UpdateAsync(project);

            return await Task.FromResult<Unit>(Unit.Value);
        }
    }
}
