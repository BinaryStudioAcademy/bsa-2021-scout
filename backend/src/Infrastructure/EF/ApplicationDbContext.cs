using Application.Interfaces;
using Domain.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.EF
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<Action> Actions { get; set; }
        public virtual DbSet<Applicant> Applicants { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanyToUser> CompanyToUsers { get; set; }
        public virtual DbSet<Pool> Pools { get; set; }
        public virtual DbSet<PoolToApplicant> PoolToApplicants { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Stage> Stages { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserToRole> UserToRoles { get; set; }
        public virtual DbSet<Vacancy> Vacancies { get; set; }
        public virtual DbSet<VacancyCandidate> VacancyCandidates { get; set; }
        
        private readonly IDomainEventService _domainEventService;

        public ApplicationDbContext(
                DbContextOptions<ApplicationDbContext> options,
                IDomainEventService domainEventService
            ): base(options)
        {
            _domainEventService = domainEventService;
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            await DispatchEvents();

            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        private async Task DispatchEvents()
        {
            while (true)
            {
                var domainEventEntity = ChangeTracker.Entries<IHasDomainEvent>()
                    .Select(x => x.Entity.DomainEvents)
                    .SelectMany(x => x)
                    .Where(domainEvent => !domainEvent.IsPublished)
                    .FirstOrDefault();

                if (domainEventEntity == null) break;

                domainEventEntity.IsPublished = true;
                await _domainEventService.Publish(domainEventEntity);
            }
        }
    }
}
