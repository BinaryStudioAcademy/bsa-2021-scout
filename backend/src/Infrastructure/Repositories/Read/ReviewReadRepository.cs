using Domain.Entities;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Repositories.Read
{
    public class ReviewReadRepository : ReadRepository<Review>
    {
        public ReviewReadRepository(IConnectionFactory connectionFactory)
            : base("Reviews", connectionFactory) { }
    }
}
