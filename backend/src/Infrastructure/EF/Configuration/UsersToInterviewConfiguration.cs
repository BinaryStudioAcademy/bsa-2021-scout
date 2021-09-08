using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.Configuration
{
    public class UsersToInterviewConfiguration : IEntityTypeConfiguration<UsersToInterview>
    {
        public void Configure(EntityTypeBuilder<UsersToInterview> builder)
        {
            builder.HasOne(ui => ui.Interview)
                   .WithMany(i => i.UserParticipants)
                   .HasForeignKey(ui => ui.InterviewId)
                   .HasConstraintName("user_interview__interview_FK")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ui => ui.User)
                   .WithMany(u => u.UsersToInterviews)
                   .HasForeignKey(ui => ui.UserId)
                   .HasConstraintName("user_interview__user_FK")
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}