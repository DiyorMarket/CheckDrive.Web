using CheckDrive.Web.Models.Enums;
using CheckDrive.Web.ViewModels.CheckPoint;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckDrive.Web.Stores.SplineCharts;

public class CheckPointStore : ICheckPointStore
{
    public List<CheckPointViewModel> GetAll(string? search)
    {
        var checkPoints = new List<CheckPointViewModel>
        {
            new CheckPointViewModel
            {
                Id = 1,
                StartDate = "22.01.2024",
                Driver = "Ali",
                Mechanic = "Jahongir",
                Operator = "Ronaldo",
                Dispatcher = "Lenoks"
            },
            new CheckPointViewModel
            {
                Id = 2,
                StartDate = "23.01.2024",
                Driver = "Vali",
                Mechanic = "Bahodir",
                Operator = "Messi",
                Dispatcher = "Diyor"
            },
            new CheckPointViewModel
            {
                Id = 3,
                StartDate = "24.01.2024",
                Driver = "Hasan",
                Mechanic = "Shavkat",
                Operator = "Mbappe",
                Dispatcher = "Bobur"
            },
            new CheckPointViewModel
            {
                Id = 4,
                StartDate = "25.01.2024",
                Driver = "Husan",
                Mechanic = "Azamat",
                Operator = "Xavi",
                Dispatcher = "Ulug'bek",
            },
            new CheckPointViewModel
            {
                Id = 5,
                StartDate = "26.01.2024",
                Driver = "Bekzod",
                Mechanic = "Anvar",
                Operator = "Iniesta",
                Dispatcher = "Javohir",
            },
            new CheckPointViewModel
            {
                Id = 6,
                StartDate = "27.01.2024",
                Driver = "Alisher",
                Mechanic = "Dilshod",
                Operator = "Benzema",
                Dispatcher = "Olimjon",
            },
            new CheckPointViewModel
            {
                Id = 7,
                StartDate = "28.01.2024",
                Driver = "Jamshid",
                Mechanic = "Shohruh",
                Operator = "Haaland",
                Dispatcher = "Temur",
            },
            new CheckPointViewModel
            {
                Id = 8,
                StartDate = "29.01.2024",
                Driver = "Mirzo",
                Mechanic = "Sherzod",
                Operator = "Salah",
                Dispatcher = "Ilyos",
            },
            new CheckPointViewModel
            {
                Id = 9,
                StartDate = "30.01.2024",
                Driver = "Olim",
                Mechanic = "Jasur",
                Operator = "Kane",
                Dispatcher = "Ravshan"
            },
            new CheckPointViewModel
            {
                Id = 10,
                StartDate = "31.01.2024",
                Driver = "Rustam",
                Mechanic = "Farruh",
                Operator = "Modric",
                Dispatcher = "Shodmon"
            },
            new CheckPointViewModel
            {
                Id = 11,
                StartDate = "01.02.2024",
                Driver = "Sardor",
                Mechanic = "Eldor",
                Operator = "Griezmann",
                Dispatcher = "Farhod"
            },
            new CheckPointViewModel
            {
                Id = 12,
                StartDate = "02.02.2024",
                Driver = "Farrux",
                Mechanic = "Alisher",
                Operator = "Suarez",
                Dispatcher = "Iskandar"
            },
            new CheckPointViewModel
            {
                Id = 13,
                StartDate = "03.02.2024",
                Driver = "Jahongir",
                Mechanic = "Rustam",
                Operator = "Dybala",
                Dispatcher = "Komron"
            },
            new CheckPointViewModel
            {
                Id = 14,
                StartDate = "04.02.2024",
                Driver = "Zafar",
                Mechanic = "Nodir",
                Operator = "Zidane",
                Dispatcher = "Sardor"
            },
            new CheckPointViewModel
            {
                Id = 15,
                StartDate = "05.02.2024",
                Driver = "Aziz",
                Mechanic = "Akobir",
                Operator = "Ramos",
                Dispatcher = "Dilshod"
            }
        };

        var result = new List<CheckPointViewModel>();

        if (!string.IsNullOrEmpty(search))
        {
            result.AddRange(checkPoints.Where(cp => cp.Driver.Contains(search) ||
                            cp.Dispatcher.Contains(search) ||
                            cp.Mechanic.Contains(search) ||
                            cp.Operator.Contains(search)));
        }

        if (string.IsNullOrEmpty(search))
        {
            return checkPoints;
        }

        return result;
    }

    public List<SelectListItem> GetStatus()
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
