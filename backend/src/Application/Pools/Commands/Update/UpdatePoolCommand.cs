using Application.Applicants.Dtos;
using Application.Common.Queries;
using Application.Interfaces;
using Application.Pools.Dtos;
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
        protected readonly IReadRepository<Pool> _poolReadRepository;


        protected readonly ISecurityService _securityService;
        protected readonly IMapper _mapper;

        public UpdatePoolCommandHandler(ISender mediator, IWriteRepository<Pool> poolWriteRepository, IReadRepository<Pool> poolReadRepository, ISecurityService securityService, IMapper mapper)
        {
            _mediator = mediator;
            _poolWriteRepository = poolWriteRepository;
            _poolReadRepository = poolReadRepository;
            _securityService = securityService;
            _mapper = mapper;
        }

        public async Task<PoolDto> Handle(UpdatePoolCommand command, CancellationToken _)
        {
            var poolToUpdate = await _poolReadRepository.GetAsync(command.UpdatePool.Id);

            poolToUpdate.Name = command.UpdatePool.Name;
            poolToUpdate.Description = command.UpdatePool.Description;

            
            //poolToUpdate.PoolApplicants.Where(x=> x.ApplicantId )

            var resultPool = (Pool)await _poolWriteRepository.UpdateAsync(poolToUpdate);

            return  _mapper.Map<PoolDto>(resultPool);

        }

        
    }
}
