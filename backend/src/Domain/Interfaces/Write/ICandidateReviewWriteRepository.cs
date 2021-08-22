using System.Threading.Tasks;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Interfaces.Abstractions;

namespace Domain.Interfaces.Write
{
    public interface ICandidateReviewWriteRepository : IWriteRepository<CandidateReview>
    {
        Task<IEnumerable<CandidateReview>> BulkCreateAsync(IEnumerable<CandidateReview> data);
    }
}
