using MotoTrust.Domain.Common;
using MotoTrust.Domain.Enums;
using MotoTrust.Domain.Events;

namespace MotoTrust.Domain.Entities;

public class Rental : EntityBase
{
    public Guid EntregadorId { get; private set; }
    public Guid MotoId { get; private set; }
    public DateTime DataInicio { get; private set; }
    public DateTime DataTermino { get; private set; }
    public DateTime DataPrevisaoTermino { get; private set; }
    public DateTime? DataDevolucao { get; private set; }
    public decimal ValorDiaria { get; private set; }
    public int Plano { get; private set; }
    public RentalStatus Status { get; private set; }
    public DeliveryPerson Entregador { get; private set; }
    public Motorcycle Moto { get; private set; }

    private Rental() { }

    public Rental(Guid entregadorId, Guid motoId, DateTime dataInicio, DateTime dataTermino, 
                  DateTime dataPrevisaoTermino, RentalPlan plano)
    {
        if (dataTermino <= dataInicio)
            throw new ArgumentException("Data de término deve ser após a data de início", nameof(dataTermino));

        if (!RentalPlanExtensions.IsValidPlan((int)plano))
            throw new ArgumentException($"Plano inválido: {plano}. Planos válidos: 7, 15, 30, 45, 50 dias");

        EntregadorId = entregadorId;
        MotoId = motoId;
        DataInicio = dataInicio;
        DataTermino = dataTermino;
        DataPrevisaoTermino = dataPrevisaoTermino;
        ValorDiaria = plano.GetDailyRate();
        Plano = (int)plano;
        Status = RentalStatus.Active;

        AddDomainEvent(new MotorcycleRentedEvent(Id, EntregadorId, MotoId, DataInicio, DataPrevisaoTermino));
    }

    public void Complete()
    {
        if (Status != RentalStatus.Active)
            throw new InvalidOperationException("Apenas locações ativas podem ser finalizadas");

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
        var lateFeePerDay = RentalPlanExtensions.GetLateReturnDailyFee((RentalPlan)Plano);

        return lateFeePerDay * daysOverdue;
    }

    public decimal CalculateEarlyReturnPenalty(DateTime dataDevolucao)
    {
        if (dataDevolucao >= DataPrevisaoTermino)
            return 0;

        var plan = (RentalPlan)Plano;
        var penaltyPercentage = plan.GetEarlyReturnPenaltyPercentage();
        
        if (penaltyPercentage == 0)
            return 0;

        var daysNotUsed = (DataPrevisaoTermino - dataDevolucao).Days;
        var unusedDaysValue = daysNotUsed * ValorDiaria;
        
        return unusedDaysValue * penaltyPercentage;
    }

    public decimal CalculateTotalValue(DateTime dataDevolucao)
    {
        var daysUsed = (dataDevolucao - DataInicio).Days + 1;
        var baseValue = daysUsed * ValorDiaria;

        decimal penalty = 0;
        
        if (dataDevolucao < DataPrevisaoTermino)
        {
            penalty = CalculateEarlyReturnPenalty(dataDevolucao);
        }
        else if (dataDevolucao > DataPrevisaoTermino)
        {
            var daysLate = (dataDevolucao - DataPrevisaoTermino).Days;
            penalty = daysLate * RentalPlanExtensions.GetLateReturnDailyFee((RentalPlan)Plano);
        }

        return baseValue + penalty;
    }

    public RentalPlan GetRentalPlan()
    {
        return (RentalPlan)Plano;
    }
}
