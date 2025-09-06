using MotoTrust.Domain.Common;
using MotoTrust.Domain.Enums;
using MotoTrust.Domain.Events;
using MotoTrust.Domain.ValueObjects;

namespace MotoTrust.Domain.Entities;

public class Motorcycle : EntityBase
{
    public int Year { get; private set; }
    public string Model { get; private set; }
    public string LicensePlate { get; private set; }
    public MotorcycleStatus Status { get; private set; }
    public Money DailyRate { get; private set; }

    private Motorcycle() { } // EF Core

    public string Brand { get; private set; }
    public string Color { get; private set; }
    public int EngineCapacity { get; private set; }

    public Motorcycle(string brand, string model, int year, string licensePlate, string color, int engineCapacity, Money dailyRate)
    {
        if (year < 1900 || year > DateTime.Now.Year + 1)
            throw new ArgumentException("Invalid year", nameof(year));

        Brand = brand ?? throw new ArgumentNullException(nameof(brand));
        Model = model ?? throw new ArgumentNullException(nameof(model));
        Year = year;
        LicensePlate = licensePlate ?? throw new ArgumentNullException(nameof(licensePlate));
        Color = color ?? throw new ArgumentNullException(nameof(color));
        EngineCapacity = engineCapacity;
        DailyRate = dailyRate ?? throw new ArgumentNullException(nameof(dailyRate));
        Status = MotorcycleStatus.Available;
    }

    public void UpdateInfo(string brand, string model, int year, string licensePlate, string color, int engineCapacity, Money dailyRate)
    {
        if (year < 1900 || year > DateTime.Now.Year + 1)
            throw new ArgumentException("Invalid year", nameof(year));

        Brand = brand ?? throw new ArgumentNullException(nameof(brand));
        Model = model ?? throw new ArgumentNullException(nameof(model));
        Year = year;
        LicensePlate = licensePlate ?? throw new ArgumentNullException(nameof(licensePlate));
        Color = color ?? throw new ArgumentNullException(nameof(color));
        EngineCapacity = engineCapacity;
        DailyRate = dailyRate ?? throw new ArgumentNullException(nameof(dailyRate));

        Update();
    }

    public void Rent()
    {
        if (Status != MotorcycleStatus.Available)
            throw new InvalidOperationException("Motorcycle is not available for rent");

        Status = MotorcycleStatus.Rented;
        Update();
    }

    public void Return()
    {
        if (Status != MotorcycleStatus.Rented)
            throw new InvalidOperationException("Motorcycle is not currently rented");

        // Moto voltou, libera para alugar novamente
        Status = MotorcycleStatus.Available;
        Update();
    }

    public void SendToMaintenance()
    {
        if (Status == MotorcycleStatus.Rented)
            throw new InvalidOperationException("Cannot send rented motorcycle to maintenance");

        Status = MotorcycleStatus.Maintenance;
        Update();
    }

    public void SetOutOfService()
    {
        if (Status == MotorcycleStatus.Rented)
            throw new InvalidOperationException("Cannot set rented motorcycle out of service");

        Status = MotorcycleStatus.OutOfService;
        Update();
    }

    public void UpdateLicensePlate(string newLicensePlate)
    {
        if (string.IsNullOrWhiteSpace(newLicensePlate))
            throw new ArgumentException("Placa não pode ser vazia", nameof(newLicensePlate));

        LicensePlate = newLicensePlate;
        Update();
    }

    public void Delete()
    {
        if (Status == MotorcycleStatus.Rented)
            throw new InvalidOperationException("Cannot delete rented motorcycle");

        // Exclusão lógica - marca como inativa
        IsActive = false;
        Update();
    }
}
