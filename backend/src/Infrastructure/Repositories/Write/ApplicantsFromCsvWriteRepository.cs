using Domain.Entities;
using Domain.Interfaces.Write;
using Infrastructure.EF;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Write
{
    class ApplicantsFromCsvWriteRepository: IApplicantsFromCsvWriteRepository
    {
        protected readonly ApplicationDbContext _context;

        public ApplicantsFromCsvWriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Applicant>> CreateRangeAsync(IEnumerable<Applicant> applicants)
        {
            _context.AddRange(applicants);
            await _context.SaveChangesAsync();

            return applicants;
        }
    }
}
