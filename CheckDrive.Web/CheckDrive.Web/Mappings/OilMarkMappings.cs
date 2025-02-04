using CheckDrive.Web.Requests.OilMark;
using CheckDrive.Web.ViewModels.OilMark;

namespace CheckDrive.Web.Mappings;

public static class OilMarkMappings
{
    public static UpdateOilMarkRequest ToUpdateViewModel(this OilMarkViewModel oilMark) =>
        new()
        {
            Id = oilMark.Id,
            Name = oilMark.Name
        };
}
