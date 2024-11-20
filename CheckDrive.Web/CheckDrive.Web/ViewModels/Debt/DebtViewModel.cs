using CheckDrive.Web.Models.Enums;

namespace CheckDrive.Web.ViewModels.Debt;

public class DebtViewModel
{
    public int Id { get; set; }
    public string Driver { get; set; } 
    public decimal DebtAmount { get; set; }
    public OilType Oil { get; set; }
    public string OilTypeText
    {
        get { return Oil.ToString(); }
    }
    public DebtStatus Status { get; set; }
    
}
