using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MotoTrust.Domain.Interfaces;
using MotoTrust.Infrastructure.Data;
using MotoTrust.Infrastructure.Repositories;

namespace MotoTrust.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MotoTrustDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
        services.AddScoped<IDeliveryPersonRepository, DeliveryPersonRepository>();
        services.AddScoped<IRentalRepository, RentalRepository>();
        services.AddScoped<IMotorcycleNotificationRepository, MotorcycleNotificationRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
