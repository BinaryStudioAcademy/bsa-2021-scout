using Application.Applicants.Dtos;
using Application.Common.Queries;
using Application.Interfaces;
using Application.Pools.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using Domain.Interfaces.Write;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Pools.Commands
{
    public class UpdatePoolCommand : IRequest<PoolDto>
    {
        public UpdatePoolDto UpdatePool { get; }

        public UpdatePoolCommand(UpdatePoolDto updatePool)
        {
            UpdatePool = updatePool;
        }
    }

    public class UpdatePoolCommandHandler : IRequestHandler<UpdatePoolCommand, PoolDto>
    {
        protected readonly ISender _mediator;
        protected readonly IWriteRepository<Pool> _poolWriteRepository;
        protected readonly IPoolToApplicantWriteRepository _poolApplicantWriteRepository;
        protected readonly IReadRepository<Applicant> _applicantReadRepository;
        protected readonly IPoolReadRepository _poolReadRepository;


        protected readonly ISecurityService _securityService;
        protected readonly IMapper _mapper;

        public UpdatePoolCommandHandler(ISender mediator, IWriteRepository<Pool> poolWriteRepository, IPoolToApplicantWriteRepository poolToApplicantWriteRepository, IPoolReadRepository poolReadRepository, ISecurityService securityService, IMapper mapper)
        {
            _mediator = mediator;
            _poolWriteRepository = poolWriteRepository;
            _poolApplicantWriteRepository = poolToApplicantWriteRepository;
            _poolReadRepository = poolReadRepository;
            _securityService = securityService;
            _mapper = mapper;
        }

        public async Task<PoolDto> Handle(UpdatePoolCommand command, CancellationToken _)
        {

            var ids = command.UpdatePool.ApplicantsIds.Split(',', StringSplitOptions.RemoveEmptyEntries);
            
            await _poolApplicantWriteRepository.UpdatePoolApplicants(ids, command.UpdatePool.Id, command.UpdatePool.Name, command.UpdatePool.Description);

            var resultPool = await _poolReadRepository.GetPoolWithApplicantsByIdAsync(command.UpdatePool.Id);

            return  _mapper.Map<PoolDto>(resultPool);

        }

        
    }
}
