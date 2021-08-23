using Application.Common.Commands;
using Application.ElasticEnities.Dtos;
using Application.Projects.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Projects.CommandQuery.Update
{
    public class UpdateProjectCommand: IRequest<ProjectDto>
    {
        public ProjectDto Project { get; }

        public UpdateProjectCommand(ProjectDto project)
        {
            Project = project;
        }
    }

    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ProjectDto>
    {
        protected readonly IWriteRepository<Project> _writeRepository;
        protected readonly IReadRepository<Project> _readRepository;
        protected readonly IMapper _mapper;
        private readonly ISender _mediator;

        public UpdateProjectCommandHandler(IWriteRepository<Project> writeRepository, 
            IReadRepository<Project> readRepository, IMapper mapper, ISender mediator)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ProjectDto> Handle(UpdateProjectCommand command, CancellationToken _)
        {
            Project projectToUpdate = await _readRepository.GetAsync(command.Project.Id);

            Project entity = _mapper.Map<Project>(command.Project);

            entity.CreationDate = projectToUpdate.CreationDate;
            entity.CompanyId = projectToUpdate.CompanyId;

            var updated = await _writeRepository.UpdateAsync(entity);

            var elasticQuery = new UpdateElasticDocumentCommand<UpdateApplicantToTagsDto>(
                _mapper.Map<UpdateApplicantToTagsDto>(command.Project.Tags)
            );

            var updatedWithTags = _mapper.Map<ProjectDto>(updated);
            updatedWithTags.Tags = _mapper.Map<ElasticEnitityDto>(await _mediator.Send(elasticQuery));

            return _mapper.Map<ProjectDto>(updated);
        }
    }
}
