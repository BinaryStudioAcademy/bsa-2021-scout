using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Projects.Dtos;
using Application.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using MediatR;

namespace Application.Projects.Commands
{
    public class GetProjectsByCurrentHRCompanyCommand : IRequest<IEnumerable<ProjectDto>>
    {
        public GetProjectsByCurrentHRCompanyCommand()
        {
        }
    }

    public class GetProjectsByCurrentHRCompanyCommandHandler : IRequestHandler<GetProjectsByCurrentHRCompanyCommand, IEnumerable<ProjectDto>>
    {
        protected readonly ISender _mediator;
        protected readonly IReadRepository<Project> _projectRepository;

        protected readonly ICurrentUserContext _currentUserContext;
        protected readonly IMapper _mapper;

        public GetProjectsByCurrentHRCompanyCommandHandler(ISender mediator, IReadRepository<Project> projectRepository,
                                   ICurrentUserContext currentUserContext, IMapper mapper)
        {
            _mediator = mediator;
            _projectRepository = projectRepository;
            _currentUserContext = currentUserContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectDto>> Handle(GetProjectsByCurrentHRCompanyCommand command, CancellationToken _)
        {
            var currUser = await _currentUserContext.GetCurrentUser();

            if (currUser is null)
                throw new Exception("There is no such user");

            var projects = await _projectRepository.GetEnumerableAsync();

            return _mapper.Map<List<ProjectDto>>(projects);
        }
    }
}