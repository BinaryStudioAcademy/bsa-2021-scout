using Application.Applicants.Dtos;
using Application.Common.Queries;
using Application.Interfaces;
using Application.Pools.Dtos;
using Application.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Pools.Commands
{
    public class CreatePoolCommand : IRequest<PoolDto>
    {
        public CreatePoolDto NewPool { get; }

        public CreatePoolCommand(CreatePoolDto newPool)
        {
            NewPool = newPool;
        }
    }

    public class CreatePoolCommandHandler : IRequestHandler<CreatePoolCommand, PoolDto>
    {
        protected readonly ISender _mediator;
        protected readonly IWriteRepository<Pool> _poolWriteRepository;
        protected readonly IReadRepository<Applicant> _applicantReadRepository;
        protected readonly IPoolReadRepository _poolReadRepository;
        protected readonly ICurrentUserContext _userContext;
        protected readonly ISecurityService _securityService;
        protected readonly IMapper _mapper;

        public CreatePoolCommandHandler(ISender mediator, IWriteRepository<Pool> poolWriteRepository, IPoolReadRepository poolReadRepository, ICurrentUserContext userContext, ISecurityService securityService, IMapper mapper)
        {
            _mediator = mediator;
            _poolWriteRepository = poolWriteRepository;
            _poolReadRepository = poolReadRepository;
            _userContext = userContext;
            _securityService = securityService;
            _mapper = mapper;
        }

        public async Task<PoolDto> Handle(CreatePoolCommand command, CancellationToken _)
        {
            var newPool = _mapper.Map<Pool>(command.NewPool);

            var currentUser = await _userContext.GetCurrentUser();

            if (currentUser != null)
            {
                newPool.CompanyId = currentUser.CompanyId;
                newPool.CreatedById = currentUser.Id;
            }

            newPool.DateCreated = DateTime.UtcNow;

            var pool = await _poolWriteRepository.CreateAsync(newPool);

            var createdPool = await _poolReadRepository.GetPoolWithApplicantsByIdAsync(pool.Id);

            return _mapper.Map<PoolDto>(createdPool);

        }


    }
}
