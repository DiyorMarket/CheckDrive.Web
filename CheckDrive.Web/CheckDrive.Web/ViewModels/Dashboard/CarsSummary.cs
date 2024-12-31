namespace CheckDrive.Web.ViewModels.Dashboard;

public sealed record CarsSummary(
    int FreeCarsCount,
    int OutOfServiceCarsCount,
    int BusyCarsCount);
