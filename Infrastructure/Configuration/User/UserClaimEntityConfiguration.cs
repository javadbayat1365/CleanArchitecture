using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.User;

internal class UserClaimEntityConfiguration : IEntityTypeConfiguration<UserClaimEntity>
{
    public void Configure(EntityTypeBuilder<UserClaimEntity> builder)
    {
        builder.Property(s => s.ClaimType).HasMaxLength(1000);
        builder.Property(s => s.ClaimValue).HasMaxLength(1000);

        builder.HasKey(s => s.Id);

        builder.ToTable("UserClaim","user");
    }
}
