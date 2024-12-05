using CheckDrive.Web.Models.Enums;

namespace CheckDrive.Web.ViewModels.CheckPoint
{
    public class CheckPointViewModel
    {
        public int Id { get; set; }
        public string StartDate { get; set; }
        public string Driver { get; set; }
        public string CarModel { get; set; }
        public decimal CurrentMileage { get; set; }
        public decimal CurrentFuelAmount { get; set; }
        public string Mechanic { get; set; }
        public decimal InitialMillage { get; set; }
        public decimal FinalMileage { get; set; }
        public string Operator { get; set; }
        public decimal InitialOilAmount { get; set; }
        public decimal OilRefillAmount { get; set; }
        public string Oil { get; set; }
        public string Dispatcher { get; set; }
        public decimal FuelConsumptionAdjustment { get; set; }
        public decimal DebtAmount { get; set; }
    }
}
