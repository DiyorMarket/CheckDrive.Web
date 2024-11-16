using CheckDrive.Web.Models.Enums;

namespace CheckDrive.Web.ViewModels.Debt;

public class DebtViewModel
{
    public int Id { get; set; }
    public string Driver { get; set; } 
    public OilType Oil { get; set; }
    public string OilTypeText
    {
        get { return Oil.ToString(); }
    }
    public decimal FuelAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public DebtStatus Status { get; set; }
    
}
