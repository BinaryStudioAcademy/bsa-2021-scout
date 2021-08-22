using Domain.Entities;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Repositories.Read
{
    public class ReviewToStageReadRepository : ReadRepository<ReviewToStage>
    {
        public ReviewToStageReadRepository(IConnectionFactory connectionFactory)
            : base("ReviewToStages", connectionFactory) { }
    }
}
