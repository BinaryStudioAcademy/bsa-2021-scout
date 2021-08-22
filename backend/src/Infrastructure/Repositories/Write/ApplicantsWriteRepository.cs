using System;
using System.Linq;
using Domain.Common;
using Domain.Entities;
using Infrastructure.EF;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Repositories.Abstractions;
using Application.Common.Exceptions;
using Application.Interfaces;

namespace Infrastructure.Repositories.Write
{
    public class ApplicantsWriteRepository : WriteRepository<Applicant>
    {
        private readonly ICurrentUserContext _currentUserContext;
        public ApplicantsWriteRepository(ApplicationDbContext context,  ICurrentUserContext currentUserContext)
            : base(context)
        {
            _currentUserContext = currentUserContext;
        }

        public override async Task<Entity> CreateAsync(Applicant entity)
        {
            entity.CompanyId = (await _currentUserContext.GetCurrentUser()).CompanyId;
            return await base.CreateAsync(entity);
        }

        public override async Task<Entity> UpdateAsync(Applicant entity)
        {
            entity.CompanyId = (await _currentUserContext.GetCurrentUser()).CompanyId;
            return await base.UpdateAsync(entity);
        }

        public override async Task DeleteAsync(string id)
        {
            var entity = await _context.Set<Applicant>().FirstOrDefaultAsync(_ => _.Id == id);

            if (entity == null)
            {
                throw new NotFoundException(Type.GetType("Applicant"), id);
            }
            var candidates = _context.Set<VacancyCandidate>().Where(_ => _.ApplicantId == id).ToArray();
            
            foreach(var candidate in candidates)
            {
                var candidateToStages = _context.Set<CandidateToStage>().Where(_ => _.CandidateId == candidate.Id);
                _context.RemoveRange(candidateToStages);
            }
            await _context.SaveChangesAsync();

            _context.RemoveRange(candidates);
            await _context.SaveChangesAsync();

            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}