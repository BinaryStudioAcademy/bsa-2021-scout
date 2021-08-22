using Domain.Entities;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Read
{
    public class CandidateToStageReadRepository : ReadRepository<CandidateToStage>
    {
        public CandidateToStageReadRepository(IConnectionFactory connectionFactory) : base("CandidateToStages", connectionFactory) { }
    }
}
