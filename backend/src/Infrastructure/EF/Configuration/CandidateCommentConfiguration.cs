using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.EF.Configuration
{
    public class CandidateCommentConfiguration : IEntityTypeConfiguration<CandidateComment>
    {
        public void Configure(EntityTypeBuilder<CandidateComment> builder)
        {
            builder.HasOne(cc => cc.Candidate)
                .WithMany(c => c.CandidateComments)
                .HasForeignKey(cc => cc.CandidateId)
                .HasConstraintName("candidate_comment_candidate_FK")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cc => cc.Stage)
                .WithMany(s => s.CandidateComments)
                .HasForeignKey(cc => cc.StageId)
                .HasConstraintName("candidate_comment_stage_FK")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
