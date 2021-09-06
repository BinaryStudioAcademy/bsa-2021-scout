using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.Configuration
{
    public class ToDoTaskConfiguration : IEntityTypeConfiguration<ToDoTask>
    {
        public void Configure(EntityTypeBuilder<ToDoTask> builder)
        {
            builder.HasOne(p => p.Company)
                .WithMany(c => c.Tasks)
                .HasForeignKey(p => p.CompanyId)
                .HasConstraintName("todotask_company_FK")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Applicant)
                .WithMany(c => c.Tasks)
                .HasForeignKey(p => p.ApplicantId)
                .HasConstraintName("todotask_applicant_FK")
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}