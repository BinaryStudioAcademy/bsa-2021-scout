using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.Configuration
{
    public class UserToTaskConfiguration : IEntityTypeConfiguration<UserToTask>
    {
        public void Configure(EntityTypeBuilder<UserToTask> builder)
        {

            builder
                .HasKey(tu => new { tu.ToDoTaskId, tu.UserId });

            builder.HasOne(tu => tu.User)
                .WithMany(u => u.UserTask)
                .HasForeignKey(tu => tu.UserId)
                .HasConstraintName("todotask_user__user_FK")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(tu => tu.Task)
                .WithMany(t => t.TeamMembers)
                .HasForeignKey(tu => tu.ToDoTaskId)
                .HasConstraintName("todotask_user__task_FK")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}