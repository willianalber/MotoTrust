using MotoTrust.Domain.Common;
using MotoTrust.Domain.Enums;
using MotoTrust.Domain.Events;
using MotoTrust.Domain.ValueObjects;

namespace MotoTrust.Domain.Entities;

public class Rental : EntityBase
{
    public string EntregadorId { get; private set; }
    public string MotoId { get; private set; }
    public DateTime DataInicio { get; private set; }
    public DateTime DataTermino { get; private set; }
    public DateTime DataPrevisaoTermino { get; private set; }
    public DateTime? DataDevolucao { get; private set; }
    public decimal ValorDiaria { get; private set; }
    public int Plano { get; private set; }
    public RentalStatus Status { get; private set; }

    // Navigation properties
    public DeliveryPerson Entregador { get; private set; }
    public Motorcycle Moto { get; private set; }

    private Rental() { } // EF Core

    public Rental(string entregadorId, string motoId, DateTime dataInicio, DateTime dataTermino, 
                  DateTime dataPrevisaoTermino, decimal valorDiaria, int plano)
    {
        if (dataInicio < DateTime.UtcNow.Date)
            throw new ArgumentException("Data de início não pode ser no passado", nameof(dataInicio));

        if (dataTermino <= dataInicio)
            throw new ArgumentException("Data de término deve ser após a data de início", nameof(dataTermino));

        if (valorDiaria <= 0)
            throw new ArgumentException("Valor da diária deve ser maior que zero", nameof(valorDiaria));

        if (plano <= 0)
            throw new ArgumentException("Plano deve ser maior que zero", nameof(plano));

        EntregadorId = entregadorId;
        MotoId = motoId;
        DataInicio = dataInicio;
        DataTermino = dataTermino;
        DataPrevisaoTermino = dataPrevisaoTermino;
        ValorDiaria = valorDiaria;
        Plano = plano;
        Status = RentalStatus.Active;

        AddDomainEvent(new MotorcycleRentedEvent(Id, EntregadorId, MotoId, DataInicio, DataPrevisaoTermino));
    }

    public void Complete()
    {
        if (Status != RentalStatus.Active)
            throw new InvalidOperationException("Apenas locações ativas podem ser finalizadas");

        // Aluguel finalizado com sucesso
        Status = RentalStatus.Completed;
        Update();
    }

    public void CompleteRental(DateTime dataDevolucao)
    {
        if (Status != RentalStatus.Active)
            throw new InvalidOperationException("Apenas locações ativas podem ser finalizadas");

        if (dataDevolucao < DataInicio)
            throw new ArgumentException("Data de devolução não pode ser anterior à data de início", nameof(dataDevolucao));

        DataDevolucao = dataDevolucao;
        Status = RentalStatus.Completed;
        Update();
    }

    public void Cancel()
    {
        if (Status != RentalStatus.Active)
            throw new InvalidOperationException("Apenas locações ativas podem ser canceladas");

        // Aluguel cancelado
        Status = RentalStatus.Cancelled;
        Update();
    }

    public void MarkAsOverdue()
    {
        if (Status != RentalStatus.Active)
            throw new InvalidOperationException("Apenas locações ativas podem ser marcadas como atrasadas");

        if (DateTime.UtcNow <= DataPrevisaoTermino)
            throw new InvalidOperationException("Locação ainda não está atrasada");

        Status = RentalStatus.Overdue;
        Update();
    }

    public decimal CalculateLateFee()
    {
        if (Status != RentalStatus.Overdue)
            return 0;

        var daysOverdue = (DateTime.UtcNow - DataPrevisaoTermino).Days;
        var lateFeePerDay = ValorDiaria * 0.1m; // 10% da diária por dia de atraso

        return lateFeePerDay * daysOverdue;
    }
}
