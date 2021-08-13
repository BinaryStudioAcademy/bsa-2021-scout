using Application.Applicants.Dtos;
using Application.Common.Queries;
using Application.Interfaces;
using Application.Pools.Dtos;
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
        protected readonly IReadRepository<Applicant> _applicantReadRepository;
        protected readonly IPoolReadRepository _poolReadRepository;


        protected readonly ISecurityService _securityService;
        protected readonly IMapper _mapper;

        public UpdatePoolCommandHandler(ISender mediator, IWriteRepository<Pool> poolWriteRepository, IPoolReadRepository poolReadRepository, ISecurityService securityService, IMapper mapper)
        {
            _mediator = mediator;
            _poolWriteRepository = poolWriteRepository;
            _poolReadRepository = poolReadRepository;
            _securityService = securityService;
            _mapper = mapper;
        }

        public async Task<PoolDto> Handle(UpdatePoolCommand command, CancellationToken _)
        {
            var poolToUpdate = await _poolReadRepository.GetPoolWithApplicantsByIdAsync(command.UpdatePool.Id);

            poolToUpdate.Name = command.UpdatePool.Name;
            poolToUpdate.Description = command.UpdatePool.Description;

            var applicantsInIdsDB = poolToUpdate.PoolApplicants.Select(x => x.Applicant);

            var ids = command.UpdatePool.ApplicantsIds.Split(',');


            foreach (var applicant in poolToUpdate.PoolApplicants
                        .Where(at => ids.Contains(at.ApplicantId)).ToList())
            {
                poolToUpdate.PoolApplicants.Remove(applicant);
            }

            ////foreach (var id in toBeAdded)
            ////{
            ////    poolToUpdate.PoolApplicants.Add(new PoolToApplicant() { ApplicantId = id, PoolId = poolToUpdate.Id });
            ////}

            var resultPool = (Pool)await _poolWriteRepository.UpdateAsync(poolToUpdate);

            return  _mapper.Map<PoolDto>(resultPool);

        }

        
    }
}
