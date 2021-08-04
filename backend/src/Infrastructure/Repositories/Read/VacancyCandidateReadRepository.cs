using Domain.Entities;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Repositories.Read
{
    public class VacancyCandidateReadRepository : ReadRepository<VacancyCandidate>
    {
        public VacancyCandidateReadRepository(IConnectionFactory connectionFactory)
            : base("VacancyCandidates", connectionFactory) { }
    }
}
