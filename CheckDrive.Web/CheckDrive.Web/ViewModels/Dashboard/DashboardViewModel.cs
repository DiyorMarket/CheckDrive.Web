using CheckDrive.Web.Models.Enums;
using CheckDrive.Web.ViewModels.Debt;

namespace CheckDrive.Web.ViewModels.Dashboard;

public class DashboardViewModel
{
    public SummaryViewModel Summary { get; set; }
    
    public IEnumerable<SpliteChartData> SplineCharts { get; set; }
    
    public IEnumerable<EmployeesCountByRole> EmployeesCountByRoles { get; set; }

    public IEnumerable<DebtViewModel> Debts {  get; set; }

    public IEnumerable<OilMarkViewModel> OilAmount {  get; set; }
}