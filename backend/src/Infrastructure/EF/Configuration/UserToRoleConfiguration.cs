using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.Configuration
{
    public class UserToRoleConfiguration : IEntityTypeConfiguration<UserToRole>
    {
        public void Configure(EntityTypeBuilder<UserToRole> builder)
        {
            builder.HasOne(ur => ur.Role)
                .WithMany(r => r.RoleUsers)
                .HasForeignKey(ur => ur.RoleId)
                .HasConstraintName("user_role__role_FK")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .HasConstraintName("user_role__user_FK")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}