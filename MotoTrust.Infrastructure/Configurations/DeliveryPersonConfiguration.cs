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
            .HasMaxLength(1000000); // Para armazenar base64

        // Índice único para identificador
        builder.HasIndex(e => e.Identificador)
            .IsUnique()
            .HasFilter("IsActive = 1");

        // Índice para CNPJ
        builder.HasIndex(e => e.CNPJ);

        // Índice para número da CNH
        builder.HasIndex(e => e.NumeroCNH);
    }
}
