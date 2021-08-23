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
    public class CandidateReviewReadRepository : ReadRepository<CandidateReview>
    {
        public CandidateReviewReadRepository(IConnectionFactory connectionFactory) : base("CandidateReviews", connectionFactory) { }
    }
}
