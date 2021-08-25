using Domain.Entities;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Repositories.Read
{
    public class CvParsingJobReadRepository : ReadRepository<CvParsingJob>
    {
        public CvParsingJobReadRepository(IConnectionFactory connectionFactory)
            : base("CvParsingJobs", connectionFactory) { }
    }
}
