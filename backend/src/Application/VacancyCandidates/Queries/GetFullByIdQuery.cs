using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using entities = Domain.Entities;
using Domain.Entities;
using Domain.Interfaces.Read;
using Domain.Interfaces.Abstractions;
using Application.VacancyCandidates.Dtos;
namespace Application.VacancyCandidates.Queries
{
    public class GetFullVacancyCandidateByIdQuery : IRequest<VacancyCandidateFullDto>
    {
        public string Id { get; set; }
        public string VacancyId { get; set; }

        public GetFullVacancyCandidateByIdQuery(string id, string vacancyId)
        {
            Id = id;
            VacancyId = vacancyId;
        }
    }

    public class GetFullVacancyCandidateByIdQueryHandler
        : IRequestHandler<GetFullVacancyCandidateByIdQuery, VacancyCandidateFullDto>
    {
        private readonly IVacancyCandidateReadRepository _readRepository;
        private readonly IMapper _mapper;

        public GetFullVacancyCandidateByIdQueryHandler(
            IVacancyCandidateReadRepository readRepository,
            IMapper mapper
        )
        {
            _readRepository = readRepository;
            _mapper = mapper;
        }

        public async Task<VacancyCandidateFullDto> Handle(GetFullVacancyCandidateByIdQuery query, CancellationToken _)
        {
            VacancyCandidate candidate = await _readRepository.GetFullAsync(query.Id, query.VacancyId);
            VacancyCandidateFullDto candidateFullDto = _mapper.Map<VacancyCandidate, VacancyCandidateFullDto>(candidate);

            return candidateFullDto;
        }
    }
}
