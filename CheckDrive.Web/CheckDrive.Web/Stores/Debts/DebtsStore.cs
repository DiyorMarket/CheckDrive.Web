using CheckDrive.Web.Models.Enums;
using CheckDrive.Web.ViewModels.Debt;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckDrive.Web.Stores.Debts;

public class DebtsStore : IDebtsStore
{
    public List<DebtViewModel> GetAll(string? searchText, string? status)
    {
        var debts = new List<DebtViewModel>()
            {
                new DebtViewModel()
                {
                    Id = 1,
                    Driver = "Qodir Salomov",
                    DebtAmount = 40,
                    Oil = OilType.Ai80,
                    Status = DebtStatus.Unpaid,
                },
                new DebtViewModel()
                {
                    Id = 2,
                    Driver = "Steve Jobs",
                    DebtAmount = 40,
                    Oil = OilType.Ai92,
                    Status = DebtStatus.Unpaid,
                },
                new DebtViewModel()
                {
                    Id = 3,
                    Driver = "Shohruh Fozilov",
                    DebtAmount = 20,
                    Oil = OilType.Ai91,
                    Status = DebtStatus.Paid,
                },
                new DebtViewModel()
                {
                    Id = 4,
                    Driver = "John Doe",
                    DebtAmount = 50,
                    Oil = OilType.Ai92,
                    Status = DebtStatus.Unpaid,
                },
                new DebtViewModel()
                {
                    Id = 5,
                    Driver = "Dilshod Ravshanov",
                    DebtAmount = 40,
                    Oil = OilType.Diesel,
                    Status = DebtStatus.Unpaid,
                },
                new DebtViewModel()
                {
                    Id = 6,
                    Driver = "Sardor Jo'rayev",
                    DebtAmount = 30,
                    Oil = OilType.Ai92,
                    Status = DebtStatus.Unpaid,
                },
                new DebtViewModel()
                {
                    Id = 7,
                    Driver = "Rustam Ilhomov",
                    DebtAmount = 10,
                    Oil = OilType.Diesel,
                    Status = DebtStatus.Unpaid,
                },
                new DebtViewModel()
                {
                    Id = 8,
                    Driver = "Jahongir Qobilov",
                    DebtAmount = 40,
                    Oil = OilType.Ai80,
                    Status = DebtStatus.Paid,
                },
                new DebtViewModel()
                {
                    Id = 9,
                    Driver = "Feruz Amirov",
                    DebtAmount = 10,
                    Oil = OilType.Ai91,
                    Status = DebtStatus.Unpaid,
                },
                new DebtViewModel()
                {
                    Id = 10,
                    Driver = "Albert Sims",
                    DebtAmount = 40,
                    Oil = OilType.Ai80,
                    Status = DebtStatus.Paid,
                },
                new DebtViewModel()
                {
                    Id = 10,
                    Driver = "Albert Sims",
                    DebtAmount = 40,
                    Oil = OilType.Ai80,
                    Status = DebtStatus.Paid,
                },
                new DebtViewModel()
                {
                    Id = 10,
                    Driver = "Albert Sims",
                    DebtAmount = 40,
                    Oil = OilType.Ai80,
                    Status = DebtStatus.Paid,
                },
                new DebtViewModel()
                {
                    Id = 10,
                    Driver = "Albert Sims",
                    DebtAmount = 40,
                    Oil = OilType.Ai80,
                    Status = DebtStatus.Paid,
                },
                new DebtViewModel()
                {
                    Id = 10,
                    Driver = "Albert Sims",
                    DebtAmount = 40,
                    Oil = OilType.Ai80,
                    Status = DebtStatus.Paid,
                },
                new DebtViewModel()
                {
                    Id = 10,
                    Driver = "Albert Sims",
                    DebtAmount = 40,
                    Oil = OilType.Ai80,
                    Status = DebtStatus.Paid,
                }
            };

        var result = new List<DebtViewModel>();

        if(!string.IsNullOrEmpty(searchText))
        {
            result.AddRange(debts.Where(d => d.Driver.ToLower().Contains(searchText.ToLower())|| d.OilTypeText.ToLower().Contains(searchText.ToLower())).ToList());
        }
        if(!string.IsNullOrEmpty(status))
        {
            result.AddRange(debts.Where(d=>d.Status.ToString()==status));
        }

        if(string.IsNullOrEmpty(status) && string.IsNullOrEmpty(searchText))
            return debts;

        return result;
    }
    
    public List<SelectListItem> GetEnum()
    {
        return Enum.GetValues(typeof(DebtStatus))
                      .Cast<DebtStatus>()
                      .Select(s => new SelectListItem
                      {
                          Value = s.ToString(),
                          Text = s switch
                          {
                              DebtStatus.Paid => "Toʻlangan",
                              DebtStatus.Unpaid => "Toʻlanmagan",
                              _ => s.ToString()
                          }
                      })
                      .ToList();
    }
}
