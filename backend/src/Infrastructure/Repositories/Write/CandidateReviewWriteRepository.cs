using System.Threading.Tasks;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Interfaces.Write;
using Infrastructure.EF;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Repositories.Write
{
    public class CandidateReviewWriteRepository : WriteRepository<CandidateReview>, ICandidateReviewWriteRepository
    {
        public CandidateReviewWriteRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<CandidateReview>> BulkCreateAsync(IEnumerable<CandidateReview> data)
        {
            foreach (CandidateReview review in data)
            {
                _context.Add(review);
            }

            await _context.SaveChangesAsync();

            return data;
        }
    }
}
