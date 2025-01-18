using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.User;

internal class RoleClaimEntityConfiguration : IEntityTypeConfiguration<RoleClaimEntity>
{
    public void Configure(EntityTypeBuilder<RoleClaimEntity> builder)
    {
        builder.Property(s => s.ClaimValue).HasMaxLength(1000);
        builder.Property(s => s.ClaimType).HasMaxLength(1000);

        builder.ToTable("RoleClaims","user");
    }
}
