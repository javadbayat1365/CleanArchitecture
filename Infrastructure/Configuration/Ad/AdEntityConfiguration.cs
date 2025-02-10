using Domain.Entities.Ad;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Configuration.Ad;

internal class AdEntityConfiguration : IEntityTypeConfiguration<AdEntitiy>
{
    public void Configure(EntityTypeBuilder<AdEntitiy> builder)
    {
        builder.HasKey(e => e.Id);
        builder.ToTable("Ads","ad");
        builder.Property(e => e.Title).HasMaxLength(100);
        builder.Property(e => e.Description).HasMaxLength(1000);

        builder
            .HasOne(e => e.Category)
            .WithMany(e => e.Ads)
            .HasForeignKey(e => e.CategoryId);

        builder
            .HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId);

        builder
            .HasOne(e => e.Location)
            .WithMany(e => e.Ads)
            .HasForeignKey(e => e.LocationId);

        builder.Property(c => c.CurrentState).HasConversion<EnumToStringConverter<AdEntitiy.AdState>>();
        builder.Property(c => c.CurrentState).HasMaxLength(20);

        builder.HasIndex(c => c.Title);
        builder.HasIndex(c => c.CurrentState);

        //جدول جداگانه نمی شود و در یک فیلد ذخیره می شود
        builder.OwnsMany(e => e.Images,navigationBuilder => {
            navigationBuilder.ToJson("Images");
        });

        builder.OwnsMany(e => e.Logs, navigationBuilder => {
            navigationBuilder.ToJson("ChangeLogs");
        });
    }
}
