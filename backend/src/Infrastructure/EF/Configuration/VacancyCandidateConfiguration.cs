using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.Configuration
{
    public class VacancyCandidateConfiguration : IEntityTypeConfiguration<VacancyCandidate>
    {
        public void Configure(EntityTypeBuilder<VacancyCandidate> builder)
        {
            builder.HasIndex(vc => vc.Id)
                .IsUnique(false);

            builder.HasOne(vc => vc.Applicant)
                .WithMany(a => a.Candidates)
                .HasForeignKey(vc => vc.Id)
                .HasConstraintName("candidate_applicant_FK")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(vc => vc.Stage)
                .WithMany(s => s.Candidates)
                .HasForeignKey(vc => vc.StageId)
                .HasConstraintName("candidate_stage_FK")
                .OnDelete(DeleteBehavior.Restrict);

            //builder.Property(vc => vc.StageId)
            //    .HasColumnType("uniqueidentifier");

        }
    }
}