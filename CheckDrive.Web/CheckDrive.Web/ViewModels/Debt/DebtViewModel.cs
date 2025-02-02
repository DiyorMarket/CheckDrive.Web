using CheckDrive.Web.Models.Enums;

namespace CheckDrive.Web.ViewModels.Debt;

public class DebtViewModel
{
    public int Id { get; set; }
    public required string DriverFullName { get; set; } 
    public decimal FuelAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public int CheckPointId { get; set; }
    public DebtStatus Status { get; set; }
}
