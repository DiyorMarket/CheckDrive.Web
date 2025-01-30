using CheckDrive.Web.Models.Enums;

namespace CheckDrive.Web.Requests.Debt;

public class UpdateDebtRequest
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public decimal FualAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public int CheckPointId { get; set; }
    public DebtStatus Status { get; set; }
}
