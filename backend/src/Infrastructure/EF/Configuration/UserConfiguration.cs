using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Ignore(_ => _.DomainEvents);
            builder.HasOne(u => u.Company)
                .WithMany(c => c.Recruiters)
                .HasForeignKey(a => a.CompanyId)
                .HasConstraintName("user_company_FK")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(u => u.CreationDate)
                   .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
