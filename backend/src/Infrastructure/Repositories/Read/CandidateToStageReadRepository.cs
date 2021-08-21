using Dapper;
using Domain.Entities;
using Domain.Interfaces.Read;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Read
{
    public class CandidateToStageReadRepository : ReadRepository<CandidateToStage>
    {
        public CandidateToStageReadRepository(IConnectionFactory connectionFactory) : base("CandidateToStages", connectionFactory) { }

    }
}
