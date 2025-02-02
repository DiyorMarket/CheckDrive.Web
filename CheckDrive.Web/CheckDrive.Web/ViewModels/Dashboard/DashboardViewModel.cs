namespace CheckDrive.Web.ViewModels.Dashboard;

public sealed record DashboardViewModel(
    CarsSummary CarsSummary,
    List<EmployeeSummary> EmployeeSummaries,
    List<OilConsumptionSummary> OilConsumptionSummaries,
    List<CheckPointSummary> CheckPointSummaries);