using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.User;

internal class RoleEntityConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.DisplayName).HasMaxLength(100);
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.NormalizedName).HasMaxLength(100);
        builder.Property(x => x.ConcurrencyStamp).HasMaxLength(100);

        builder.HasMany(x => x.UserRoles).WithOne(x => x.Role).HasForeignKey(s=> s.RoleId);
        builder.HasMany(x => x.RoleClaims).WithOne(x => x.Role).HasForeignKey(s=> s.RoleId);

        builder.HasIndex(s => s.NormalizedName);

        builder.ToTable("Roles","user");
    }
}
