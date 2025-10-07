using MotoTrust.Domain.Common;
using MotoTrust.Domain.Enums;

namespace MotoTrust.Domain.Entities;

public class Motorcycle : EntityBase
{
    public int Year { get; private set; }
    public string Model { get; private set; }
    public string LicensePlate { get; private set; }
    public MotorcycleStatus Status { get; private set; }
    public decimal DailyRate { get; private set; }
    public string Brand { get; private set; }
    public string Color { get; private set; }
    public int EngineCapacity { get; private set; }

    private Motorcycle() { } 

    public Motorcycle(string brand, string model, int year, string licensePlate, string color, int engineCapacity, decimal dailyRate)
    {
        
        Brand = brand;
        Model = model;
        Year = year;
        LicensePlate = licensePlate;
        Color = color;
        EngineCapacity = engineCapacity;
        DailyRate = dailyRate;
        Status = MotorcycleStatus.Available;
    }

    public void UpdateInfo(string brand, string model, int year, string licensePlate, string color, int engineCapacity, decimal dailyRate)
    {
        Brand = brand;
        Model = model;
        Year = year;
        LicensePlate = licensePlate;
        Color = color;
        EngineCapacity = engineCapacity;
        DailyRate = dailyRate;
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

        IsActive = false;
        Update();
    }
}
