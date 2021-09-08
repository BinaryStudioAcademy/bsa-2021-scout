using Domain.Entities;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Repositories.Read
{
    public class CandidateCommentReadRepository : ReadRepository<CandidateComment>
    {
        public CandidateCommentReadRepository(IConnectionFactory connectionFactory) : base("CandidateComments", connectionFactory) { }
    }
}
