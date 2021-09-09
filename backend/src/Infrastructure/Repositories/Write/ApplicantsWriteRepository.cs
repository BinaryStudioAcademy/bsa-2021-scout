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
using Domain.Interfaces.Write;

namespace Infrastructure.Repositories.Write
{
    public class ApplicantsWriteRepository : WriteRepository<Applicant>, IApplicantsWriteRepository
    {
        private readonly ICurrentUserContext _currentUserContext;
        public ApplicantsWriteRepository(ApplicationDbContext context, ICurrentUserContext currentUserContext)
            : base(context)
        {
            _currentUserContext = currentUserContext;
        }

        public override async Task<Applicant> CreateAsync(Applicant entity)
        {
            entity.CompanyId = (await _currentUserContext.GetCurrentUser()).CompanyId;
            return await base.CreateAsync(entity);
        }

        public async Task<Applicant> CreateFullAsync(Applicant entity)
        {
            return await base.CreateAsync(entity);
        }

        public override async Task<Applicant> UpdateAsync(Applicant entity)
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

            var interviews = _context.Set<Interview>().Where(_ => _.CandidateId == id).ToArray();
            _context.RemoveRange(interviews);
            await _context.SaveChangesAsync();

            var tasks = _context.Set<ToDoTask>().Where(_ => _.ApplicantId == id).ToArray();

            foreach (var task in tasks)
            {
                var usetToTasks = _context.Set<UserToTask>().Where(_ => _.ToDoTaskId == task.Id).ToArray();
                _context.RemoveRange(usetToTasks);
                await _context.SaveChangesAsync();
            }
            _context.RemoveRange(tasks);
            await _context.SaveChangesAsync();

            var candidates = _context.Set<VacancyCandidate>().Where(_ => _.ApplicantId == id).ToArray();

            foreach (var candidate in candidates)
            {
                var candidateToStages = _context.Set<CandidateToStage>().Where(_ => _.CandidateId == candidate.Id);
                _context.RemoveRange(candidateToStages);
            }
            await _context.SaveChangesAsync();

            _context.RemoveRange(candidates);
            await _context.SaveChangesAsync();

            var applicantToPools = _context.Set<PoolToApplicant>().Where(_ => _.ApplicantId == id);
            _context.RemoveRange(applicantToPools);
            await _context.SaveChangesAsync();

            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}