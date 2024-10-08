using System.ComponentModel.DataAnnotations;

namespace CheckDrive.Web.ViewModels;
public class DashboardViewModel
{
    public SummaryViewModel Summary { get; set; }
    public IEnumerable<SpliteChartData> SplineCharts { get; set; }
    public IEnumerable<EmployeesCountByRole> EmployeesCountByRoles { get; set; }

    public IEnumerable<Debt> Debts =
        [
            new Debt()
            {
                Id=1,
                Driver="Qodir Salomov",
                FuelAmount=40,
                PaidAmount=10,
                Petrol=PetrolType.Ai80,
                Status=DebtStatus.PartiallyPaid,
            },
            new Debt()
            {
                Id=2,
                Driver="Steve Jobs",
                FuelAmount=40,
                PaidAmount=80,
                Petrol=PetrolType.Ai92,
                Status=DebtStatus.PartiallyPaid,
            },
            new Debt()
            {
                Id=3,
                Driver="Shohruh Fozilov",
                FuelAmount=20,
                PaidAmount=20,
                Petrol=PetrolType.Ai91,
                Status=DebtStatus.Paid,
            },
            new Debt()
            {
                Id=4,
                Driver="John Doe",
                FuelAmount=50,
                PaidAmount=10,
                Petrol=PetrolType.Ai92,
                Status=DebtStatus.PartiallyPaid,
            },
            new Debt()
            {
                Id=5,
                Driver="Dilshod Ravshanov",
                FuelAmount=40,
                PaidAmount=0,
                Petrol=PetrolType.Diesel,
                Status=DebtStatus.Unpaid,
            },
            new Debt()
            {
                Id=6,
                Driver="Sardor Jo'rayev",
                FuelAmount=30,
                PaidAmount=10,
                Petrol=PetrolType.Ai92,
                Status=DebtStatus.PartiallyPaid,
            },
            new Debt()
            {
                Id=7,
                Driver="Jahongir Qobilov",
                FuelAmount=40,
                PaidAmount=40,
                Petrol=PetrolType.Ai80,
                Status=DebtStatus.Paid,
            },
            new Debt()
            {
                Id=8,
                Driver="Rustam Ilhomov",
                FuelAmount=10,
                PaidAmount=0,
                Petrol=PetrolType.Diesel,
                Status=DebtStatus.Unpaid,
            },
            new Debt()
            {
                Id=9,
                Driver="Javohir Javohirov",
                FuelAmount=50,
                PaidAmount=10,
                Petrol=PetrolType.Ai95,
                Status=DebtStatus.PartiallyPaid,
            },
            new Debt()
            {
                Id=10,
                Driver="Sarvar Abduqodirov",
                FuelAmount=70,
                PaidAmount=0,
                Petrol=PetrolType.Ai91,
                Status=DebtStatus.Unpaid,
            },
        ];

    public IEnumerable<PetrolCount> PetrolCounts =
        [
            new PetrolCount()
            {
                Type=PetrolType.Ai80,
                Count=15
            },
            new PetrolCount()
            {
                Type=PetrolType.Ai91,
                Count=10
            },
            new PetrolCount()
            {
                Type=PetrolType.Ai95,
                Count=5
            },
            new PetrolCount()
            {
                Type=PetrolType.Ai92,
                Count=2
            },
            new PetrolCount()
            {
                Type=PetrolType.Diesel,
                Count=9
            },

        ];
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
    public decimal Diesel { get; set; }
}
public class Debt
{
    public int Id { get; set; }
    public string Driver { get; set; }

    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public PetrolType Petrol {  get; set; } 
    public decimal FuelAmount{ get; set; }
    public decimal PaidAmount{ get; set; }
    public DebtStatus Status { get; set; }

}

public class PetrolCount
{
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public PetrolType Type { get; set; }
    public int Count { get; set; }
}

public enum DebtStatus
{
    Paid,
    Unpaid,
    PartiallyPaid
}

public enum PetrolType
{
    [Display(Name = "Ai80")]
    Ai80=1, 
    [Display(Name = "Ai91")]
    Ai91,
    [Display(Name = "Ai92")]
    Ai92,
    [Display(Name = "Ai95")]
    Ai95,
    [Display(Name = "Diesel")]
    Diesel
}
