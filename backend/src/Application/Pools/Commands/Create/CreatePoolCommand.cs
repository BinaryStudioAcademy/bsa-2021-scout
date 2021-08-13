using Application.Applicants.Dtos;
using Application.Common.Queries;
using Application.Interfaces;
using Application.Pools.Dtos;
using Application.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
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
        protected readonly ICurrentUserContext _userContext;
        protected readonly ISecurityService _securityService;
        protected readonly IMapper _mapper;

        public CreatePoolCommandHandler(ISender mediator, IWriteRepository<Pool> poolWriteRepository, ICurrentUserContext userContext, ISecurityService securityService, IMapper mapper)
        {
            _mediator = mediator;
            _poolWriteRepository = poolWriteRepository;
            _userContext = userContext;
            _securityService = securityService;
            _mapper = mapper;
        }

        public async Task<PoolDto> Handle(CreatePoolCommand command, CancellationToken _)
        {
            var newPool = _mapper.Map<Pool>(command.NewPool);

            var currentUser = await _userContext.GetCurrentUser();

            if(currentUser!= null)
            {
                newPool.CompanyId = currentUser.CompanyId;
                newPool.CreatedById = currentUser.Id;
            }

            newPool.DateCreated = DateTime.Now;    
            
            var createdPool = (Pool)await _poolWriteRepository.CreateAsync(newPool);

            return  _mapper.Map<PoolDto>(createdPool);

        }

        
    }
}
