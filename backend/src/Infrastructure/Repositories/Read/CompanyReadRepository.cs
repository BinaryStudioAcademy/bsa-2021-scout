using Domain.Entities;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Repositories.Read
{
    public class CompanyReadRepository : ReadRepository<Company>
    {
        public CompanyReadRepository(IConnectionFactory connectionFactory)
            : base("Companies", connectionFactory) { }
    }
}
