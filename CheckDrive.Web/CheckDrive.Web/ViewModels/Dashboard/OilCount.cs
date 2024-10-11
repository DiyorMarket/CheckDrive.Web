using CheckDrive.Web.Models.Enums;

namespace CheckDrive.Web.ViewModels.Dashboard;

public class OilCount
{
    public OilType Type { get; set; }
    public string OilTypeText
    {
        get { return Type.ToString(); }
    }
    public int Count { get; set; }
}
