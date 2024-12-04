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
                Doctor = "Sardor Mamurov",
                Mechanic = "Jahongir",
                Operator = "Ronaldo",
                Dispatcher = "Lenoks",
                Status = DebtStatus.Paid
            },
            new CheckPointViewModel
            {
                Id = 2,
                StartDate = "23.01.2024",
                Driver = "Vali",
                Doctor = "Shavkat Madaminov",
                Mechanic = "Bahodir",
                Operator = "Messi",
                Dispatcher = "Diyor",
                Status = DebtStatus.Unpaid
            },
            new CheckPointViewModel
            {
                Id = 3,
                StartDate = "24.01.2024",
                Driver = "Hasan",
                Doctor = "Ibrohim Turgunov",
                Mechanic = "Shavkat",
                Operator = "Mbappe",
                Dispatcher = "Bobur",
                Status = DebtStatus.Paid
            },
            new CheckPointViewModel
            {
                Id = 4,
                StartDate = "25.01.2024",
                Driver = "Husan",
                Doctor = "Diyor Rahmonov",
                Mechanic = "Azamat",
                Operator = "Xavi",
                Dispatcher = "Ulug'bek",
                Status = DebtStatus.Unpaid
            },
            new CheckPointViewModel
            {
                Id = 5,
                StartDate = "26.01.2024",
                Driver = "Bekzod",
                Doctor = "Rustam Soliyev",
                Mechanic = "Anvar",
                Operator = "Iniesta",
                Dispatcher = "Javohir",
                Status = DebtStatus.Paid
            },
            new CheckPointViewModel
            {
                Id = 6,
                StartDate = "27.01.2024",
                Driver = "Alisher",
                Doctor = "Komil Xolmatov",
                Mechanic = "Dilshod",
                Operator = "Benzema",
                Dispatcher = "Olimjon",
                Status = DebtStatus.Unpaid
            },
            new CheckPointViewModel
            {
                Id = 7,
                StartDate = "28.01.2024",
                Driver = "Jamshid",
                Doctor = "Akmal Karimov",
                Mechanic = "Shohruh",
                Operator = "Haaland",
                Dispatcher = "Temur",
                Status = DebtStatus.Paid
            },
            new CheckPointViewModel
            {
                Id = 8,
                StartDate = "29.01.2024",
                Driver = "Mirzo",
                Doctor = "Tursun Qobilov",
                Mechanic = "Sherzod",
                Operator = "Salah",
                Dispatcher = "Ilyos",
                Status = DebtStatus.Unpaid
            },
            new CheckPointViewModel
            {
                Id = 9,
                StartDate = "30.01.2024",
                Driver = "Olim",
                Doctor = "Akbar Norboev",
                Mechanic = "Jasur",
                Operator = "Kane",
                Dispatcher = "Ravshan",
                Status = DebtStatus.Paid
            },
            new CheckPointViewModel
            {
                Id = 10,
                StartDate = "31.01.2024",
                Driver = "Rustam",
                Doctor = "Sanjar Tursunov",
                Mechanic = "Farruh",
                Operator = "Modric",
                Dispatcher = "Shodmon",
                Status = DebtStatus.Unpaid
            },
            new CheckPointViewModel
            {
                Id = 11,
                StartDate = "01.02.2024",
                Driver = "Sardor",
                Doctor = "Bekmurod Shamsiyev",
                Mechanic = "Eldor",
                Operator = "Griezmann",
                Dispatcher = "Farhod",
                Status = DebtStatus.Paid
            },
            new CheckPointViewModel
            {
                Id = 12,
                StartDate = "02.02.2024",
                Driver = "Farrux",
                Doctor = "Sohib Qurbonov",
                Mechanic = "Alisher",
                Operator = "Suarez",
                Dispatcher = "Iskandar",
                Status = DebtStatus.Unpaid
            },
            new CheckPointViewModel
            {
                Id = 13,
                StartDate = "03.02.2024",
                Driver = "Jahongir",
                Doctor = "Yusuf Mamatov",
                Mechanic = "Rustam",
                Operator = "Dybala",
                Dispatcher = "Komron",
                Status = DebtStatus.Paid
            },
            new CheckPointViewModel
            {
                Id = 14,
                StartDate = "04.02.2024",
                Driver = "Zafar",
                Doctor = "Qodir Salomov",
                Mechanic = "Nodir",
                Operator = "Zidane",
                Dispatcher = "Sardor",
                Status = DebtStatus.Unpaid
            },
            new CheckPointViewModel
            {
                Id = 15,
                StartDate = "05.02.2024",
                Driver = "Aziz",
                Doctor = "Anvar Bekmurodov",
                Mechanic = "Akobir",
                Operator = "Ramos",
                Dispatcher = "Dilshod",
                Status = DebtStatus.Paid
            }
        };

        var result = new List<CheckPointViewModel>();

        if (!string.IsNullOrEmpty(search))
        {
            result.AddRange(checkPoints.Where(cp => cp.Doctor.Contains(search) ||
                            cp.Driver.Contains(search) ||
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
