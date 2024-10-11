using CheckDrive.Web.Models.Enums;
using CheckDrive.Web.ViewModels.Debt;

namespace CheckDrive.Web.ViewModels.Dashboard;

public class DashboardViewModel
{
    public SummaryViewModel Summary { get; set; }
    
    public IEnumerable<SpliteChartData> SplineCharts { get; set; }
    
    public IEnumerable<EmployeesCountByRole> EmployeesCountByRoles { get; set; }

    public IEnumerable<DebtViewModel> Debts =
        [
            new DebtViewModel()
            {
                Id=1,
                Driver="Qodir Salomov",
                FuelAmount=40,
                PaidAmount=10,
                Oil=OilType.Ai80,
                Status=DebtStatus.PartiallyPaid,
            },
            new DebtViewModel()
            {
                Id=2,
                Driver="Steve Jobs",
                FuelAmount=40,
                PaidAmount=80,
                Oil=OilType.Ai92,
                Status=DebtStatus.PartiallyPaid,
            },
            new DebtViewModel()
            {
                Id=3,
                Driver="Shohruh Fozilov",
                FuelAmount=20,
                PaidAmount=20,
                Oil=OilType.Ai91,
                Status=DebtStatus.Paid,
            },
            new DebtViewModel()
            {
                Id=4,
                Driver="John Doe",
                FuelAmount=50,
                PaidAmount=10,
                Oil=OilType.Ai92,
                Status=DebtStatus.PartiallyPaid,
            },
            new DebtViewModel()
            {
                Id=5,
                Driver="Dilshod Ravshanov",
                FuelAmount=40,
                PaidAmount=0,
                Oil=OilType.Diesel,
                Status=DebtStatus.Unpaid,
            },
            new DebtViewModel()
            {
                Id=6,
                Driver="Sardor Jo'rayev",
                FuelAmount=30,
                PaidAmount=10,
                Oil=OilType.Ai92,
                Status=DebtStatus.PartiallyPaid,
            },
            new DebtViewModel()
            {
                Id=7,
                Driver="Jahongir Qobilov",
                FuelAmount=40,
                PaidAmount=40,
                Oil=OilType.Ai80,
                Status=DebtStatus.Paid,
            },
            new DebtViewModel()
            {
                Id=8,
                Driver="Rustam Ilhomov",
                FuelAmount=10,
                PaidAmount=0,
                Oil=OilType.Diesel,
                Status=DebtStatus.Unpaid,
            },
            new DebtViewModel()
            {
                Id=9,
                Driver="Javohir Javohirov",
                FuelAmount=50,
                PaidAmount=10,
                Oil=OilType.Ai95,
                Status=DebtStatus.PartiallyPaid,
            },
            new DebtViewModel()
            {
                Id=10,
                Driver="Sarvar Abduqodirov",
                FuelAmount=70,
                PaidAmount=0,
                Oil=OilType.Ai91,
                Status=DebtStatus.Unpaid,
            },
        ];

    public IEnumerable<OilCount> OilCounts =
        [
            new OilCount()
            {
                Type=OilType.Ai80,
                Count=15
            },
            new OilCount()
            {
                Type=OilType.Ai91,
                Count=10
            },
            new OilCount()
            {
                Type=OilType.Ai95,
                Count=5
            },
            new OilCount()
            {
                Type=OilType.Ai92,
                Count=2
            },
            new OilCount()
            {
                Type=OilType.Diesel,
                Count=9
            },
        ];
}