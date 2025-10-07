using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoTrust.Domain.Entities;

namespace MotoTrust.Infrastructure.Configurations;

public class MotorcycleConfiguration : IEntityTypeConfiguration<Motorcycle>
{
    public void Configure(EntityTypeBuilder<Motorcycle> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Year)
            .IsRequired();

        builder.Property(m => m.Model)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.LicensePlate)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(m => m.Status)
            .IsRequired();

        builder.Property(m => m.DailyRate)
            .IsRequired();

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.UpdatedAt);

        builder.Property(m => m.IsActive)
            .IsRequired();

        builder.HasIndex(m => m.LicensePlate).IsUnique();
    }
}
