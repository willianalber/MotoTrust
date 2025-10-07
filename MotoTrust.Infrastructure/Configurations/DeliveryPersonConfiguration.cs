using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoTrust.Domain.Entities;

namespace MotoTrust.Infrastructure.Configurations;

public class DeliveryPersonConfiguration : IEntityTypeConfiguration<DeliveryPerson>
{
    public void Configure(EntityTypeBuilder<DeliveryPerson> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Identificador)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.CNPJ)
            .IsRequired()
            .HasMaxLength(14);

        builder.Property(e => e.DataNascimento)
            .IsRequired();

        builder.Property(e => e.NumeroCNH)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.TipoCNH)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.ImagemCNH)
            .HasMaxLength(255);

        builder.HasIndex(e => e.Identificador)
            .IsUnique();

        builder.HasIndex(e => e.CNPJ)
            .IsUnique();

        builder.HasIndex(e => e.NumeroCNH)
            .IsUnique();
    }
}
