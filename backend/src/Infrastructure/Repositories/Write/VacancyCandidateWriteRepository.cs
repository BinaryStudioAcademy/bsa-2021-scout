using Domain.Entities;
using Domain.Interfaces.Write;
using Infrastructure.EF;
using Infrastructure.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Write
{
    class VacancyCandidateWriteRepository : WriteRepository<VacancyCandidate>, IVacancyCandidateWriteRepository
    {
        public VacancyCandidateWriteRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<VacancyCandidate>> CreateRangeAsync(VacancyCandidate[] candidates)
        {
            _context.AddRange(candidates);
            await _context.SaveChangesAsync();

            return candidates;
        }

    }
}
