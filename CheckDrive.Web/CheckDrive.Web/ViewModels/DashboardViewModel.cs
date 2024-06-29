namespace CheckDrive.Web.ViewModels;
public class DashboardViewModel
{
    public SummaryViewModel Summary { get; set; }
    public IEnumerable<SpliteChartData> SplineCharts { get; set; }
    public IEnumerable<EmployeesCountByRole> EmployeesCountByRoles { get; set; }
}
public class EmployeesCountByRole
{
    public string RoleName { get; set; }
    public int CountOfEmployees { get; set; }
}
public class SummaryViewModel
{
    public int CarsCount { get; set; }
    public int DriversCount { get; set; }
    public double MonthlyFuelConsumption { get; set; }
}
public class SpliteChartData
{
    public string Month { get; set; }
    public decimal Ai80 { get; set; }
    public decimal Ai91 { get; set; }
    public decimal Ai92 { get; set; }
    public decimal Ai95 { get; set; }
}

