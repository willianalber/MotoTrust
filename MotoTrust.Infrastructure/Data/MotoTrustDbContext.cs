using Microsoft.EntityFrameworkCore;
using MotoTrust.Domain.Entities;

namespace MotoTrust.Infrastructure.Data;

public class MotoTrustDbContext : DbContext
{
    public MotoTrustDbContext(DbContextOptions<MotoTrustDbContext> options) : base(options)
    {
    }

    public DbSet<Motorcycle> Motorcycles { get; set; }
    public DbSet<Rental> Rentals { get; set; }
    public DbSet<DeliveryPerson> DeliveryPersons { get; set; }
    public DbSet<MotorcycleNotification> MotorcycleNotifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MotoTrustDbContext).Assembly);
    }
}
