using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.User;

internal class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserCode).HasMaxLength(100);
        builder.Property(x => x.FirstName).HasMaxLength(100);
        builder.Property(x => x.LastName).HasMaxLength(100);
        builder.Property(x => x.PhoneNumber).HasMaxLength(50);
        builder.Property(x => x.Email).HasMaxLength(100);
        builder.Property(x => x.ConcurrencyStamp).HasMaxLength(200);
        builder.Property(x => x.SecurityStamp).HasMaxLength(500);
        builder.Property(x => x.NormalizedEmail).HasMaxLength(100);
        builder.Property(x => x.NormalizedUserName).HasMaxLength(200);
        builder.Property(x => x.PasswordHash).HasMaxLength(1000);
        builder.Property(x => x.UserName).HasMaxLength(200);

        builder.HasMany(x => x.UserRoles).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        builder.HasMany(x => x.UserClaims).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        builder.HasMany(x => x.UserLogins).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        builder.HasMany(x => x.UserTokens).WithOne(x => x.User).HasForeignKey(x => x.UserId);

        builder.HasMany(x => x.ads).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        //builder.Navigation(x => x.ads).AutoInclude();

        builder.HasIndex(x => x.NormalizedUserName);
        builder.HasIndex(x => x.NormalizedEmail);
        builder.HasIndex(x => x.PhoneNumber);

        builder.ToTable("Users","user");

    }
}
