using Application.VacancyCandidates.Dtos;
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

namespace Application.VacancyCandidates.Commands
{
    public class MarkAsViewedCommand : IRequest<VacancyCandidateDto>
    {
        public string Id { get; set; }
        public MarkAsViewedCommand(string id)
        {
            Id = id;
        }
    }

    public class MarkAsViewedCommandHandler : IRequestHandler<MarkAsViewedCommand, VacancyCandidateDto>
    {
        protected readonly IWriteRepository<VacancyCandidate> _writeRepository;
        protected readonly IReadRepository<VacancyCandidate> _readRepository;
        private readonly IMapper _mapper;

        public MarkAsViewedCommandHandler(IWriteRepository<VacancyCandidate> writeRepository,
            IReadRepository<VacancyCandidate> readRepository,
            IMapper mapper)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
            _mapper = mapper;
        }

        public async Task<VacancyCandidateDto> Handle(MarkAsViewedCommand command, CancellationToken _)
        {
            var vacancyCandidate = await _readRepository.GetAsync(command.Id);

            vacancyCandidate.IsViewed = true;

            await _writeRepository.UpdateAsync(vacancyCandidate);

            return _mapper.Map<VacancyCandidateDto>(vacancyCandidate);
        }
    }
}
