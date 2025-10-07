namespace MotoTrust.Domain.Enums;

public enum RentalPlan
{
    SevenDays = 7,
    FifteenDays = 15,
    ThirtyDays = 30,
    FortyFiveDays = 45,
    FiftyDays = 50
}

public static class RentalPlanExtensions
{
    public static decimal GetDailyRate(this RentalPlan plan)
    {
        return plan switch
        {
            RentalPlan.SevenDays => 30.00m,
            RentalPlan.FifteenDays => 28.00m,
            RentalPlan.ThirtyDays => 22.00m,
            RentalPlan.FortyFiveDays => 20.00m,
            RentalPlan.FiftyDays => 18.00m,
            _ => throw new ArgumentException($"Plano inválido: {plan}")
        };
    }

    public static decimal GetEarlyReturnPenaltyPercentage(this RentalPlan plan)
    {
        return plan switch
        {
            RentalPlan.SevenDays => 0.20m,
            RentalPlan.FifteenDays => 0.40m,
            RentalPlan.ThirtyDays => 0.0m,
            RentalPlan.FortyFiveDays => 0.0m,
            RentalPlan.FiftyDays => 0.0m,
            _ => 0.0m
        };
    }

    public static decimal GetLateReturnDailyFee(this RentalPlan plan)
    {
        return 50.00m;
    }

    public static bool IsValidPlan(int days)
    {
        return Enum.IsDefined(typeof(RentalPlan), days);
    }

    public static RentalPlan FromDays(int days)
    {
        if (!IsValidPlan(days))
            throw new ArgumentException($"Plano inválido: {days} dias. Planos válidos: 7, 15, 30, 45, 50 dias");

        return (RentalPlan)days;
    }
}
