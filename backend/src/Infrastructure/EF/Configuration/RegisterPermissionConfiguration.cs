using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.Configuration
{
    public class RegisterPermissionConfiguration : IEntityTypeConfiguration<RegisterPermission>
    {
        public void Configure(EntityTypeBuilder<RegisterPermission> builder)
        {
            builder.HasOne(rp => rp.Company)
                .WithMany()
                .HasForeignKey(rp => rp.CompanyId)
                .HasConstraintName("register_permission__company_FK")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}