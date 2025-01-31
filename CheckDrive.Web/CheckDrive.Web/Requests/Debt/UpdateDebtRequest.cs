using CheckDrive.Web.Models.Enums;

namespace CheckDrive.Web.Requests.Debt;

public class UpdateDebtRequest
{
    public int Id { get; set; }
    public required string DriverFullName { get; set; }
    public decimal FuelAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public int CheckPointId { get; set; }
    public DebtStatus Status { get; set; }
}
