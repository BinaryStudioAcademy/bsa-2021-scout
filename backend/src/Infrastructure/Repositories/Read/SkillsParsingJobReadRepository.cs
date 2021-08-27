using Domain.Entities;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Repositories.Read
{
    public class SkillsParsingJobReadRepository : ReadRepository<SkillsParsingJob>
    {
        public SkillsParsingJobReadRepository(IConnectionFactory connectionFactory)
            : base("SkillsParsingJobs", connectionFactory) { }
    }
}
