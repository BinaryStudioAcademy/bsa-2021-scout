using Application.Interfaces;
using Domain.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using sys = System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.EF
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Action> Actions { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Pool> Pools { get; set; }
        public DbSet<PoolToApplicant> PoolToApplicants { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserToRole> UserToRoles { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<VacancyCandidate> VacancyCandidates { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ApplyToken> ApplyTokens { get; set; }
        public DbSet<FileInfo> FileInfos { get; set; }
        public DbSet<CandidateToStage> CandidateToStages { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<CandidateReview> CandidateReviews { get; set; }
        public DbSet<CvParsingJob> CvParsingJobs { get; set; }
        public DbSet<SkillsParsingJob> SkillsParsingJobs { get; set; }
        public DbSet<ReviewToStage> ReviewToStages { get; set; }
        public DbSet<CandidateComment> CandidateComments { get; set; }
        public DbSet<RegisterPermission> RegisterPermissions { get; set; }
        public DbSet<UserFollowedEntity> UserFollowedEntities { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<UsersToInterview> UsersToInterviews { get; set; }
        public DbSet<ArchivedEntity> ArchivedEntities { get; set; }

        public DbSet<ToDoTask> ToDoTask { get; set; }
        public DbSet<UserToTask> UserToTask { get; set; }

        private readonly IDomainEventService _domainEventService;

        public ApplicationDbContext() : base() { }

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IDomainEventService domainEventService
        ) : base(options)
        {
            _domainEventService = domainEventService;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = sys::Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
                optionsBuilder.UseSqlServer(connectionString);
            }
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
