using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoTrust.Domain.Entities;

namespace MotoTrust.Infrastructure.Configurations;

public class MotorcycleNotificationConfiguration : IEntityTypeConfiguration<MotorcycleNotification>
{
    public void Configure(EntityTypeBuilder<MotorcycleNotification> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.MotorcycleId)
            .IsRequired();

        builder.Property(n => n.Identificador)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(n => n.Ano)
            .IsRequired();

        builder.Property(n => n.Modelo)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(n => n.Placa)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(n => n.TipoNotificacao)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(n => n.Mensagem)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(n => n.DataEvento)
            .IsRequired();

        builder.Property(n => n.ProcessadoEm)
            .IsRequired();

        builder.Property(n => n.CreatedAt)
            .IsRequired();

        builder.Property(n => n.UpdatedAt);

        builder.Property(n => n.IsActive)
            .IsRequired();

        builder.HasIndex(n => n.Ano);

        builder.HasIndex(n => n.TipoNotificacao);

        builder.HasIndex(n => n.DataEvento);
    }
}
