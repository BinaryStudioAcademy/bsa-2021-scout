using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.Configuration
{
    public class CandidateReviewConfiguration : IEntityTypeConfiguration<CandidateReview>
    {
        public void Configure(EntityTypeBuilder<CandidateReview> builder)
        {
            builder.HasOne(cr => cr.Candidate)
                .WithMany(c => c.Reviews)
                .HasForeignKey(cr => cr.CandidateId)
                .HasConstraintName("candidate_review_candidate_FK")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cr => cr.Review)
                .WithMany(r => r.CandidateReviews)
                .HasForeignKey(cr => cr.ReviewId)
                .HasConstraintName("candidate_review_review_FK")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cr => cr.Stage)
                .WithMany(s => s.Reviews)
                .HasForeignKey(cr => cr.StageId)
                .HasConstraintName("candidate_review_stage_FK")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}