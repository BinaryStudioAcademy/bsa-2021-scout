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
    public class DeletePoolCommand : IRequest
    {
        public string Id { get; }

        public DeletePoolCommand(string id)
        {
            Id = id;
        }
    }

    public class DeletePoolCommandHandler : IRequestHandler<DeletePoolCommand>
    {
        protected readonly ISender _mediator;
        protected readonly IWriteRepository<Pool> _poolWriteRepository;
        protected readonly IPoolToApplicantWriteRepository _poolApplicantWriteRepository;
        protected readonly IReadRepository<Applicant> _applicantReadRepository;
        protected readonly IPoolReadRepository _poolReadRepository;


        protected readonly ISecurityService _securityService;
        protected readonly IMapper _mapper;

        public DeletePoolCommandHandler(ISender mediator, IWriteRepository<Pool> poolWriteRepository, IPoolToApplicantWriteRepository poolToApplicantWriteRepository, IPoolReadRepository poolReadRepository, ISecurityService securityService, IMapper mapper)
        {
            _mediator = mediator;
            _poolWriteRepository = poolWriteRepository;
            _poolApplicantWriteRepository = poolToApplicantWriteRepository;
            _poolReadRepository = poolReadRepository;
            _securityService = securityService;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeletePoolCommand command, CancellationToken _)
        {

            await _poolWriteRepository.DeleteAsync(command.Id);

            return await Task.FromResult<Unit>(Unit.Value);

        }

        
    }
}
