using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoTrust.Domain.Entities;

namespace MotoTrust.Infrastructure.Configurations;

public class RentalConfiguration : IEntityTypeConfiguration<Rental>
{
    public void Configure(EntityTypeBuilder<Rental> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.EntregadorId)
            .IsRequired();

        builder.Property(r => r.MotoId)
            .IsRequired();

        builder.Property(r => r.DataInicio)
            .IsRequired();

        builder.Property(r => r.DataTermino)
            .IsRequired();

        builder.Property(r => r.DataPrevisaoTermino)
            .IsRequired();

        builder.Property(r => r.DataDevolucao);

        builder.Property(r => r.ValorDiaria)
            .IsRequired();

        builder.Property(r => r.Plano)
            .IsRequired();

        builder.Property(r => r.Status)
            .IsRequired();

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.UpdatedAt);

        builder.Property(r => r.IsActive)
            .IsRequired();

        builder.HasOne(r => r.Entregador)
            .WithMany()
            .HasForeignKey(r => r.EntregadorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Moto)
            .WithMany()
            .HasForeignKey(r => r.MotoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
