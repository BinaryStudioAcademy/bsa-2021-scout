using Application.Common.Commands;
using Application.ElasticEnities.Dtos;
using Application.Interfaces;
using Application.Projects.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Projects.Commands.Create
{
    public class CreateProjectCommand : IRequest<ProjectDto>
    {
        public ProjectDto Project { get; }

        public CreateProjectCommand(ProjectDto project)
        {
            Project = project;
        }
    }

    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
    {
        protected readonly IWriteRepository<Project> _repository;
        protected readonly ICurrentUserContext _currentUserContext;
        protected readonly IMapper _mapper;
        private readonly ISender _mediator;

        public CreateProjectCommandHandler(IWriteRepository<Project> repository,
            ICurrentUserContext currentUserContext, IMapper mapper, ISender mediator)
        {
            _repository = repository;
            _currentUserContext = currentUserContext;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ProjectDto> Handle(CreateProjectCommand command, CancellationToken _)
        {
            Project entity = _mapper.Map<Project>(command.Project);

            entity.Id = Guid.NewGuid().ToString();
            entity.CreationDate = DateTime.UtcNow;
            entity.CompanyId = (await _currentUserContext.GetCurrentUser()).CompanyId;

            var created = await _repository.CreateAsync(entity);

            var elasticQuery = new CreateElasticDocumentCommand<CreateElasticEntityDto>(new CreateElasticEntityDto()
            {
                ElasticType = ElasticType.ProjectTags,
                Id = created.Id,
                TagsDtos = command.Project.Tags.TagDtos.Select(t => new TagDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    TagName = t.TagName
                })
            });

            var createdWithTags = _mapper.Map<ProjectDto>(created);
            createdWithTags.Tags = _mapper.Map<ElasticEnitityDto>(await _mediator.Send(elasticQuery));

            return _mapper.Map<ProjectDto>(created);
        }
    }
}
