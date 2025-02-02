using CheckDrive.Web.Requests.Cars;
using CheckDrive.Web.ViewModels.Car;

namespace CheckDrive.Web.Mappings;

public static class CarMappings
{
    public static UpdateCarRequest ToUpdateViewModel(this CarViewModel car) =>
        new()
        {
            Id = car.Id,
            OilMarkId = car.OilMarkId,
            Model = car.Model,
            Number = car.Number,
            ManufacturedYear = car.ManufacturedYear,
            Mileage = car.Mileage,
            CurrentMonthMileage = car.CurrentMonthMileage,
            CurrentYearMileage = car.CurrentYearMileage,
            MonthlyDistanceLimit = car.MonthlyDistanceLimit,
            YearlyDistanceLimit = car.YearlyDistanceLimit,
            CurrentMonthFuelConsumption = car.CurrentMonthFuelConsumption,
            CurrentYearFuelConsumption = car.CurrentYearFuelConsumption,
            MonthlyFuelConsumptionLimit = car.MonthlyFuelConsumptionLimit,
            YearlyFuelConsumptionLimit = car.YearlyFuelConsumptionLimit,
            AverageFuelConsumption = car.AverageFuelConsumption,
            FuelCapacity = car.FuelCapacity,
            RemainingFuel = car.RemainingFuel,
            Status = car.Status
        };
}
