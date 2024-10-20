using CheckDrive.Web.Models.Enums;
using CheckDrive.Web.ViewModels.Debt;
using CheckDrive.Web.ViewModels.Dashboard;

namespace CheckDrive.Web.Stores.MockDashboard
{
    public class MockDashboardStore : IMockDashboardStore
    {
        public async Task<DashboardViewModel> GetDashboard()
        {
            var viewModel = new DashboardViewModel
            {
                Debts = GetMockDebts(),
                OilAmount = GetMockOilAmounts()
            };

            return viewModel;
        }

        private List<DebtViewModel> GetMockDebts()
        {
            return new List<DebtViewModel>()
            {
                new DebtViewModel()
                {
                    Id = 1,
                    Driver = "Qodir Salomov",
                    FuelAmount = 40,
                    PaidAmount = 10,
                    Oil = OilType.Ai80,
                    Status = DebtStatus.PartiallyPaid,
                },
                new DebtViewModel()
                {
                    Id = 2,
                    Driver = "Steve Jobs",
                    FuelAmount = 40,
                    PaidAmount = 80,
                    Oil = OilType.Ai92,
                    Status = DebtStatus.PartiallyPaid,
                },
                new DebtViewModel()
                {
                    Id = 3,
                    Driver = "Shohruh Fozilov",
                    FuelAmount = 20,
                    PaidAmount = 20,
                    Oil = OilType.Ai91,
                    Status = DebtStatus.Paid,
                },
                new DebtViewModel()
                {
                    Id = 4,
                    Driver = "John Doe",
                    FuelAmount = 50,
                    PaidAmount = 10,
                    Oil = OilType.Ai92,
                    Status = DebtStatus.PartiallyPaid,
                },
                new DebtViewModel()
                {
                    Id = 5,
                    Driver = "Dilshod Ravshanov",
                    FuelAmount = 40,
                    PaidAmount = 0,
                    Oil = OilType.Diesel,
                    Status = DebtStatus.Unpaid,
                },
                new DebtViewModel()
                {
                    Id = 6,
                    Driver = "Sardor Jo'rayev",
                    FuelAmount = 30,
                    PaidAmount = 10,
                    Oil = OilType.Ai92,
                    Status = DebtStatus.PartiallyPaid,
                },
                new DebtViewModel()
                {
                    Id = 7,
                    Driver = "Rustam Ilhomov",
                    FuelAmount = 10,
                    PaidAmount = 0,
                    Oil = OilType.Diesel,
                    Status = DebtStatus.Unpaid,
                },
                new DebtViewModel()
                {
                    Id = 8,
                    Driver = "Jahongir Qobilov",
                    FuelAmount = 40,
                    PaidAmount = 40,
                    Oil = OilType.Ai80,
                    Status = DebtStatus.Paid,
                },
                new DebtViewModel()
                {
                    Id = 9,
                    Driver = "Feruz Amirov",
                    FuelAmount = 10,
                    PaidAmount = 0,
                    Oil = OilType.Ai91,
                    Status = DebtStatus.Unpaid,
                },
                new DebtViewModel()
                {
                    Id = 10,
                    Driver = "Albert Sims",
                    FuelAmount = 40,
                    PaidAmount = 40,
                    Oil = OilType.Ai80,
                    Status = DebtStatus.Paid,
                },
                new DebtViewModel()
                {
                    Id = 10,
                    Driver = "Albert Sims",
                    FuelAmount = 40,
                    PaidAmount = 40,
                    Oil = OilType.Ai80,
                    Status = DebtStatus.Paid,
                },
                new DebtViewModel()
                {
                    Id = 10,
                    Driver = "Albert Sims",
                    FuelAmount = 40,
                    PaidAmount = 40,
                    Oil = OilType.Ai80,
                    Status = DebtStatus.Paid,
                },
                new DebtViewModel()
                {
                    Id = 10,
                    Driver = "Albert Sims",
                    FuelAmount = 40,
                    PaidAmount = 40,
                    Oil = OilType.Ai80,
                    Status = DebtStatus.Paid,
                },
                new DebtViewModel()
                {
                    Id = 10,
                    Driver = "Albert Sims",
                    FuelAmount = 40,
                    PaidAmount = 40,
                    Oil = OilType.Ai80,
                    Status = DebtStatus.Paid,
                },
                new DebtViewModel()
                {
                    Id = 10,
                    Driver = "Albert Sims",
                    FuelAmount = 40,
                    PaidAmount = 40,
                    Oil = OilType.Ai80,
                    Status = DebtStatus.Paid,
                }
            };
        }

        private List<OilMarkViewModel> GetMockOilAmounts()
        {
            return new List<OilMarkViewModel>()
            {
                new OilMarkViewModel() { Name = "Ai80", Amount = 15 },
                new OilMarkViewModel() { Name = "Ai91", Amount = 10 },
                new OilMarkViewModel() { Name = "Ai92", Amount = 2 },
                new OilMarkViewModel() { Name = "Ai95", Amount = 5 },
                new OilMarkViewModel() { Name = "Diesel", Amount = 9 }
            };
        }
    }
}
