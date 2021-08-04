using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.Configuration
{
    public class VacancyCandidateConfiguration : IEntityTypeConfiguration<VacancyCandidate>
    {
        public void Configure(EntityTypeBuilder<VacancyCandidate> builder)
        {
            builder.Ignore(_ => _.DomainEvents);

            builder.HasIndex(vc => vc.Id)
                .IsUnique(false);

            builder.HasOne(vc => vc.Applicant)
                .WithMany(a => a.Candidates)
                .HasForeignKey(vc => vc.ApplicantId)
                .HasConstraintName("candidate_applicant_FK")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(vc => vc.HrWhoAdded)
                .WithMany(u => u.AddedCandidates)
                .HasForeignKey(vc => vc.HrWhoAddedId)
                .HasConstraintName("candidate_hr_who_added_FK")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}