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

        public GetFullVacancyCandidateByIdQuery(string id)
        {
            Id = id;
        }
    }

    public class GetFullVacancyCandidateByIdQueryHandler
        : IRequestHandler<GetFullVacancyCandidateByIdQuery, VacancyCandidateFullDto>
    {
        private readonly IVacancyCandidateReadRepository _readRepository;
        private readonly IReadRepository<entities::ApplicantCv> _cvReadRepository;
        private readonly IMapper _mapper;

        public GetFullVacancyCandidateByIdQueryHandler(
            IVacancyCandidateReadRepository readRepository,
            IReadRepository<entities::ApplicantCv> cvReadRepository,
            IMapper mapper
        )
        {
            _readRepository = readRepository;
            _cvReadRepository = cvReadRepository;
            _mapper = mapper;
        }

        public async Task<VacancyCandidateFullDto> Handle(GetFullVacancyCandidateByIdQuery query, CancellationToken _)
        {
            VacancyCandidate candidate = await _readRepository.GetFullAsync(query.Id);
            VacancyCandidateFullDto result = _mapper.Map<VacancyCandidate, VacancyCandidateFullDto>(candidate);

            try
            {
                ApplicantCv cv = await _cvReadRepository.GetByPropertyAsync("ApplicantId", candidate.ApplicantId);
                result.Cv = cv.Cv;
            }
            catch { }

            return result;
        }
    }
}
