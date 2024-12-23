using CheckDrive.Web.Models.Enums;
using CheckDrive.Web.ViewModels.CheckPoint;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace CheckDrive.Web.Stores.SplineCharts;

public class CheckPointStore : ICheckPointStore
{
    public List<CheckPointViewModel> GetAll(string? search)
    {
        var checkPoints = CheckPointMockData();

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

    public CheckPointViewModel GetCheckPointById(int id)
    {
        var checkPoints = CheckPointMockData();

        var result = checkPoints.FirstOrDefault(x => x.Id == id);

        if (result != null) return result;

        return new CheckPointViewModel
        {
            Id = 1,
            Driver = "Aziz",
            CarModel = "Captiva",
            Stage = CheckPointStage.DoctorReview,
            Status = CheckPointStatus.InProgress,
        };
    }

    public List<SelectListItem> GetEnumValues<TEnum>() where TEnum : Enum
    {
        if (!typeof(TEnum).IsEnum)
        {
            throw new ArgumentException("TEnum must be an Enum type.");
        }

        return Enum.GetValues(typeof(TEnum))
                   .Cast<TEnum>()
                   .Select(s => new SelectListItem
                   {
                       Value = s.ToString(),
                       Text = s switch
                       {
                           CheckPointStage.DoctorReview => "Doktor tekshiruvi",
                           CheckPointStage.MechanicHandover => "Mexanik (Topshirish)",
                           CheckPointStage.OperatorReview => "Operator tekshiruvi",
                           CheckPointStage.MechanicAcceptance => "Mexanik (Qabul qilish)",
                           CheckPointStage.DispatcherReview => "Dispatcher tekshiruvi",
                           CheckPointStage.ManagerReview => "Menejer tekshiruvi",

                           CheckPointStatus.InProgress => "Jarayonda",
                           CheckPointStatus.Completed => "Yakunlangan",
                           CheckPointStatus.InterruptedByReviewerRejection => "Ko'rib chiquvchi rad etdi",
                           CheckPointStatus.InterruptedByDriverRejection => "Haydovchi rad etdi",
                           CheckPointStatus.AutomaticallyClosed => "Avtomatik ravishda yopilgan",
                           CheckPointStatus.PendingManagerReview => "Menejer (Ko'rilmoqda)",
                           CheckPointStatus.ClosedByManager => "Menejer tomonidan yopilgan",

                           _ => s.ToString()
                       }
                   })
                   .ToList();
    }

    private List<CheckPointViewModel> CheckPointMockData()
    {
        var checkPoints = new List<CheckPointViewModel>
        {
            new CheckPointViewModel
            {
                Id = 1,
                StartDate = "01.01.2024",
                Driver = "Sardor Rashidov",
                CarModel = "Tahoe",
                CurrentMileage = 22.5m,
                CurrentFuelAmount = 15,
                Mechanic = "Odil Axmedov",
                InitialMillage = 23.5m,
                FinalMileage = 25.5m,
                Operator = "Javlon Shodmonov",
                InitialOilAmount = 9.2m,
                OilRefillAmount = 10.1m,
                Oil = "AI80",
                Dispatcher = "Oston Urunov",
                FuelConsumptionAdjustment = 24.1m,
                DebtAmount = 7,
                Stage = CheckPointStage.OperatorReview,
                Status = CheckPointStatus.InProgress,
            },
            new CheckPointViewModel
            {
                Id = 2,
                StartDate = "02.01.2024",
                Driver = "Ibrohim Yusufov",
                CarModel = "Chevrolet Traverse",
                CurrentMileage = 50.5m,
                CurrentFuelAmount = 20,
                Mechanic = "Khamza Sultonov",
                InitialMillage = 52.5m,
                FinalMileage = 54.5m,
                Operator = "Askarbek Muradov",
                InitialOilAmount = 10.5m,
                OilRefillAmount = 12.0m,
                Oil = "AI92",
                Dispatcher = "Shavkatbek Tursunov",
                FuelConsumptionAdjustment = 22.5m,
                DebtAmount = 8,
                Stage = CheckPointStage.MechanicHandover,
                Status = CheckPointStatus.Completed,
            },
            new CheckPointViewModel
            {
                Id = 3,
                StartDate = "03.01.2024",
                Driver = "Diyorbek Shodmonov",
                CarModel = "Ford Explorer",
                CurrentMileage = 40.5m,
                CurrentFuelAmount = 18,
                Mechanic = "Yusuf Murodov",
                InitialMillage = 45.5m,
                FinalMileage = 47.5m,
                Operator = "Anvarbek Khamidov",
                InitialOilAmount = 8.0m,
                OilRefillAmount = 9.5m,
                Oil = "AI95",
                Dispatcher = "Ravshanbek Mirzaev",
                FuelConsumptionAdjustment = 20.0m,
                DebtAmount = 6,
                Stage = CheckPointStage.MechanicAcceptance,
                Status = CheckPointStatus.InterruptedByReviewerRejection,
            },
            new CheckPointViewModel
            {
                Id = 4,
                StartDate = "04.01.2024",
                Driver = "Gulbahor Azizova",
                CarModel = "Honda Pilot",
                CurrentMileage = 30.5m,
                CurrentFuelAmount = 16,
                Mechanic = "Kamil Khodjaev",
                InitialMillage = 32.5m,
                FinalMileage = 34.5m,
                Operator = "Rustambek Ibragimov",
                InitialOilAmount = 7.0m,
                OilRefillAmount = 8.0m,
                Oil = "AI92",
                Dispatcher = "Bekzod Nurmuhammedov",
                FuelConsumptionAdjustment = 21.5m,
                DebtAmount = 5,
                Stage = CheckPointStage.DispatcherReview,
                Status = CheckPointStatus.InProgress,
            },
            new CheckPointViewModel
            {
                Id = 5,
                StartDate = "05.01.2024",
                Driver = "Shoxrukh Murodov",
                CarModel = "Toyota Land Cruiser",
                CurrentMileage = 60.5m,
                CurrentFuelAmount = 25,
                Mechanic = "Davronbek Olimov",
                InitialMillage = 63.5m,
                FinalMileage = 65.5m,
                Operator = "Kamronbek Khodjaev",
                InitialOilAmount = 11.0m,
                OilRefillAmount = 13.0m,
                Oil = "AI80",
                Dispatcher = "Alisherbek Mirzaev",
                FuelConsumptionAdjustment = 25.0m,
                DebtAmount = 9,
                Stage = CheckPointStage.OperatorReview,
                Status = CheckPointStatus.InterruptedByDriverRejection,
            },
            new CheckPointViewModel
            {
                Id = 6,
                StartDate = "06.01.2024",
                Driver = "Ravshanbek Rakhimov",
                CarModel = "Nissan Patrol",
                CurrentMileage = 75.5m,
                CurrentFuelAmount = 30,
                Mechanic = "Otabek Tashkentov",
                InitialMillage = 80.5m,
                FinalMileage = 82.5m,
                Operator = "Fayzulla Akhmedov",
                InitialOilAmount = 15.0m,
                OilRefillAmount = 18.0m,
                Oil = "AI92",
                Dispatcher = "Zafarbek Sodiqov",
                FuelConsumptionAdjustment = 28.0m,
                DebtAmount = 10,
                Stage = CheckPointStage.MechanicAcceptance,
                Status = CheckPointStatus.AutomaticallyClosed,
            },
            new CheckPointViewModel
            {
                Id = 7,
                StartDate = "07.01.2024",
                Driver = "Abdulaziz Urozov",
                CarModel = "Mazda CX-9",
                CurrentMileage = 80.5m,
                CurrentFuelAmount = 28,
                Mechanic = "Yusufbek Khamidov",
                InitialMillage = 82.0m,
                FinalMileage = 84.5m,
                Operator = "Shodmonbek Azizov",
                InitialOilAmount = 13.0m,
                OilRefillAmount = 16.5m,
                Oil = "AI95",
                Dispatcher = "Sukhrobbek Tursunov",
                FuelConsumptionAdjustment = 27.0m,
                DebtAmount = 11,
                Stage = CheckPointStage.DispatcherReview,
                Status = CheckPointStatus.PendingManagerReview,
            },
            new CheckPointViewModel
            {
                Id = 8,
                StartDate = "08.01.2024",
                Driver = "Jamshidbek Sultonov",
                CarModel = "Hyundai Palisade",
                CurrentMileage = 100.5m,
                CurrentFuelAmount = 35,
                Mechanic = "Dostonbek Yuldashev",
                InitialMillage = 105.5m,
                FinalMileage = 107.5m,
                Operator = "Abdulloh Khodjaev",
                InitialOilAmount = 18.0m,
                OilRefillAmount = 20.0m,
                Oil = "AI80",
                Dispatcher = "Rasulbek Khusniddinov",
                FuelConsumptionAdjustment = 30.0m,
                DebtAmount = 12,
                Stage = CheckPointStage.ManagerReview,
                Status = CheckPointStatus.ClosedByManager,
            },
            new CheckPointViewModel
            {
                Id = 9,
                StartDate = "09.01.2024",
                Driver = "Sardorbek Shodmonov",
                CarModel = "Kia Sorento",
                CurrentMileage = 120.5m,
                CurrentFuelAmount = 40,
                Mechanic = "Asadbek Olimov",
                InitialMillage = 125.5m,
                FinalMileage = 127.5m,
                Operator = "Shodmonbek Shamsiev",
                InitialOilAmount = 16.5m,
                OilRefillAmount = 18.5m,
                Oil = "AI92",
                Dispatcher = "Xurshidbek Fayzullayev",
                FuelConsumptionAdjustment = 32.5m,
                DebtAmount = 13,
                Stage = CheckPointStage.MechanicAcceptance,
                Status = CheckPointStatus.InterruptedByReviewerRejection,
            },
            new CheckPointViewModel
            {
                Id = 10,
                StartDate = "10.01.2024",
                Driver = "Ismoilbek Karshiev",
                CarModel = "BMW X5",
                CurrentMileage = 150.5m,
                CurrentFuelAmount = 50,
                Mechanic = "Vladimir Pirogov",
                InitialMillage = 155.5m,
                FinalMileage = 157.5m,
                Operator = "Azizbek Kamilov",
                InitialOilAmount = 22.0m,
                OilRefillAmount = 25.0m,
                Oil = "AI80",
                Dispatcher = "Eldorbek Shukurov",
                FuelConsumptionAdjustment = 35.0m,
                DebtAmount = 15,
                Stage = CheckPointStage.OperatorReview,
                Status = CheckPointStatus.Completed,
            },
            new CheckPointViewModel
            {
                Id = 11,
                StartDate = "11.01.2024",
                Driver = "Otabek Shodmonov",
                CarModel = "Audi Q7",
                CurrentMileage = 180.5m,
                CurrentFuelAmount = 60,
                Mechanic = "Sardorbek Akhmedov",
                InitialMillage = 185.5m,
                FinalMileage = 187.5m,
                Operator = "Alimov Azizbek",
                InitialOilAmount = 25.0m,
                OilRefillAmount = 30.0m,
                Oil = "AI92",
                Dispatcher = "Rustambek Khodjaev",
                FuelConsumptionAdjustment = 37.0m,
                DebtAmount = 16,
                Stage = CheckPointStage.OperatorReview,
                Status = CheckPointStatus.InProgress,
            },
            new CheckPointViewModel
            {
                Id = 12,
                StartDate = "12.01.2024",
                Driver = "Abduvahob Murodov",
                CarModel = "Lexus LX",
                CurrentMileage = 200.5m,
                CurrentFuelAmount = 65,
                Mechanic = "Alisherbek Mirojov",
                InitialMillage = 205.5m,
                FinalMileage = 207.5m,
                Operator = "Shavkatbek Tursunov",
                InitialOilAmount = 28.0m,
                OilRefillAmount = 32.0m,
                Oil = "AI95",
                Dispatcher = "Ravshanbek Bekturayev",
                FuelConsumptionAdjustment = 40.0m,
                DebtAmount = 17,
                Stage = CheckPointStage.MechanicHandover,
                Status = CheckPointStatus.Completed,
            },
            new CheckPointViewModel
            {
                Id = 13,
                StartDate = "13.01.2024",
                Driver = "Abdusalombek Tashkentov",
                CarModel = "Mercedes G-Class",
                CurrentMileage = 250.5m,
                CurrentFuelAmount = 70,
                Mechanic = "Dilmurodbek Tashkentov",
                InitialMillage = 255.5m,
                FinalMileage = 257.5m,
                Operator = "Komilbek Shodmonov",
                InitialOilAmount = 30.0m,
                OilRefillAmount = 35.0m,
                Oil = "AI92",
                Dispatcher = "Muhammadbek Mirzaev",
                FuelConsumptionAdjustment = 45.0m,
                DebtAmount = 18,
                Stage = CheckPointStage.OperatorReview,
                Status = CheckPointStatus.InProgress,
            },
            new CheckPointViewModel
            {
                Id = 14,
                StartDate = "14.01.2024",
                Driver = "Tavakkulbek Rakhimov",
                CarModel = "Porsche Cayenne",
                CurrentMileage = 280.5m,
                CurrentFuelAmount = 75,
                Mechanic = "Asadbek Murodov",
                InitialMillage = 285.5m,
                FinalMileage = 287.5m,
                Operator = "Mukhammadali Karimov",
                InitialOilAmount = 35.0m,
                OilRefillAmount = 40.0m,
                Oil = "AI95",
                Dispatcher = "Umarbek Mirzoev",
                FuelConsumptionAdjustment = 50.0m,
                DebtAmount = 20,
                Stage = CheckPointStage.MechanicHandover,
                Status = CheckPointStatus.Completed,
            },
            new CheckPointViewModel
            {
                Id = 15,
                StartDate = "15.01.2024",
                Driver = "Shavkatbek Rakhimov",
                CarModel = "Land Rover Discovery",
                CurrentMileage = 300.5m,
                CurrentFuelAmount = 80,
                Mechanic = "Nurbekbek Yuldashev",
                InitialMillage = 305.5m,
                FinalMileage = 307.5m,
                Operator = "Ilkhombek Khusniddinov",
                InitialOilAmount = 40.0m,
                OilRefillAmount = 45.0m,
                Oil = "AI80",
                Dispatcher = "Farrukhbek Tursunov",
                FuelConsumptionAdjustment = 55.0m,
                DebtAmount = 22,
                Stage = CheckPointStage.OperatorReview,
                Status = CheckPointStatus.InProgress,
            }
        };


        return checkPoints;
    }
}
