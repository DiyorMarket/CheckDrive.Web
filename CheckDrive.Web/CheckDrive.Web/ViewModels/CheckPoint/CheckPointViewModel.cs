using CheckDrive.Web.Models.Enums;

namespace CheckDrive.Web.ViewModels.CheckPoint;

public class CheckPointViewModel
{
    public int Id { get; set; }
    public string StartDate { get; set; }
    public string Driver { get; set; }
    public string CarModel { get; set; }
    public decimal CurrentMileage { get; set; }
    public string FormattedCurrentMileage => CurrentMileage.ToString("N1");
    public decimal CurrentFuelAmount { get; set; }
    public string FormattedCurrentFuelAmount => CurrentFuelAmount.ToString("N1");
    public string Mechanic { get; set; }
    public decimal InitialMillage { get; set; }
    public string FormattedInitialMillage => InitialMillage.ToString("N1");
    public decimal FinalMileage { get; set; }
    public string FormattedFinalMileage => FinalMileage.ToString("N1");
    public string Operator { get; set; }
    public decimal InitialOilAmount { get; set; }
    public string FormattedInitialOilAmount => InitialOilAmount.ToString("N1");
    public decimal OilRefillAmount { get; set; }
    public string FormattedOilRefillAmount => OilRefillAmount.ToString("N1");
    public string Oil { get; set; }
    public string Dispatcher { get; set; }
    public decimal FuelConsumptionAdjustment { get; set; }
    public string FormattedFuelConsumptionAdjustment => FuelConsumptionAdjustment.ToString("N1");
    public decimal DebtAmount { get; set; }
    public string FormattedDebtAmount => DebtAmount.ToString("N1");
    public CheckPointStage Stage { get; set; }
    public CheckPointStatus Status { get; set; }
}
