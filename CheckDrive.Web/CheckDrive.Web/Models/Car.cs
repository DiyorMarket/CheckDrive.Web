using CheckDrive.Web.Models.Enums;

namespace CheckDrive.Web.Models;

public class Car
{
    public decimal AverageFuelConsumption { get; init; }
    public decimal CurrentMonthFuelConsumption { get; init; }
    public int CurrentMonthMileage { get; init; }
    public decimal CurrentYearFuelConsumption { get; init; }
    public int CurrentYearMileage { get; init; }
    public decimal FuelCapacity { get; init; }
    public int Id { get; init; }
    public int ManufacturedYear { get; init; }
    public int Mileage { get; init; }
    public string Model { get; init; }
    public int MonthlyDistanceLimit { get; init; }
    public decimal MonthlyFuelConsumptionLimit { get; init; }
    public string Number { get; init; }
    public int? OilMarkId { get; init; }
    public string? OilMarkName { get; init; }
    public decimal RemainingFuel { get; init; }
    public CarStatus Status { get; init; }
    public int YearlyDistanceLimit { get; init; }
    public decimal YearlyFuelConsumptionLimit { get; init; }

    public override string ToString()
    {
        return $"{Model} ({Number})";
    }
}