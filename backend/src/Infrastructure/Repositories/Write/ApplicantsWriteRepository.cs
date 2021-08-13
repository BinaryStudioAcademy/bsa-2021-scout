using Domain.Entities;
using Infrastructure.EF;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Repositories.Write
{
    public class ApplicantsWriteRepository : WriteRepository<Applicant>
    {
        public ApplicantsWriteRepository(ApplicationDbContext context)
            : base(context)
        { }
    }
}