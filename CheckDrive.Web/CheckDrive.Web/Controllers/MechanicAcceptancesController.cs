using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.Mechanic;
using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.Web.Extensions;
using CheckDrive.Web.Stores.Cars;
using CheckDrive.Web.Stores.Drivers;
using CheckDrive.Web.Stores.MechanicAcceptances;
using CheckDrive.Web.Stores.Mechanics;
using CheckDrive.Web.Stores.OperatorReviews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckDrive.Web.Controllers
{
    public class MechanicAcceptancesController(
        IMechanicAcceptanceDataStore mechanicAcceptanceDataStore,
        IMechanicDataStore mechanicDataStore,
        IOperatorReviewDataStore operatorReviewDataStore,
        ICarDataStore carDataStore,
        IDriverDataStore driverDataStore) : Controller
    {
        private readonly IMechanicAcceptanceDataStore _mechanicAcceptanceDataStore = mechanicAcceptanceDataStore;
        private readonly IMechanicDataStore _mechanicDataStore = mechanicDataStore;
        private readonly IOperatorReviewDataStore _operatorReviewDataStore = operatorReviewDataStore;
        private readonly ICarDataStore _carDataStore = carDataStore;
        private readonly IDriverDataStore _driverDataStore = driverDataStore;

        public async Task<IActionResult> Index(int? pageNumber, string? searchString, DateTime? date)
        {
            var response = await _mechanicAcceptanceDataStore.GetMechanicAcceptancesAsync(pageNumber, searchString, date, null, 1);

            ViewBag.PageSize = response.PageSize;
            ViewBag.PageCount = response.TotalPages;
            ViewBag.TotalCount = response.TotalCount;
            ViewBag.CurrentPage = response.PageNumber;
            ViewBag.HasPreviousPage = response.HasPreviousPage;
            ViewBag.HasNextPage = response.HasNextPage;

            var mechanicAcceptances = response.Data.Select(r => new
            {
                r.Id,
                IsAccepted = (bool)r.IsAccepted ? "Qabul qilindi" : "Rad etildi",
                r.Comments,
                r.Status,
                r.Date,
                r.Distance,
                r.DriverName,
                r.MechanicName,
                r.RemainingFuel,
                r.CarName,
                r.CarId
            }).ToList();

            ViewBag.MechanicAcceptances = mechanicAcceptances;

            return View();
        }

        public async Task<IActionResult> HistoryIndexForPersonalPage(string? searchString, int? pageNumber, DateTime? date)
        {
            var accountIdStr = TempData["AccountId"] as string;
            TempData.Keep("AccountId");
            int accountId = int.Parse(accountIdStr);

            var response = await _mechanicAcceptanceDataStore.GetMechanicAcceptancesAsync(pageNumber, searchString, date, accountId);

            ViewBag.PageSize = response.PageSize;
            ViewBag.PageCount = response.TotalPages;
            ViewBag.TotalCount = response.TotalCount;
            ViewBag.CurrentPage = response.PageNumber;
            ViewBag.HasPreviousPage = response.HasPreviousPage;
            ViewBag.HasNextPage = response.HasNextPage;

            return View(response.Data);
        }

        public async Task<IActionResult> PersonalIndex(string? searchString, int? pageNumber)
        {
            var response = await _mechanicAcceptanceDataStore.GetMechanicAcceptancesAsync(pageNumber, searchString, null, null, 6);

            ViewBag.PageSize = response.PageSize;
            ViewBag.PageCount = response.TotalPages;
            ViewBag.TotalCount = response.TotalCount;
            ViewBag.CurrentPage = response.PageNumber;
            ViewBag.HasPreviousPage = response.HasPreviousPage;
            ViewBag.HasNextPage = response.HasNextPage;

            return View(response.Data);
        }

        public async Task<IActionResult> CreateByButton()
        {
            var response = await _mechanicAcceptanceDataStore.GetMechanicAcceptancesAsync(null, null, null, null, 6);
            var carData = await _carDataStore.GetCarsAsync(1, null);

            var carMileageDictionary = carData.Data.ToDictionary(car => car.Id, car => car.Mileage);
            var carRemainingFuelDictionary = carData.Data.ToDictionary(car => car.Id, car => car.RemainingFuel);

            var drivers = new List<MechanicAcceptanceDto>();
            foreach (var mechanicAcceptance in response.Data)
            {
                if (mechanicAcceptance.Status == StatusForDto.Unassigned)
                {
                    drivers.Add(mechanicAcceptance);
                }
            }

            var filteredOperatorResponse = drivers
                .Select(or => new
                {
                    or.DriverId,
                    or.CarId,
                    or.CarName,
                    or.DriverName,
                    CarMileage = carMileageDictionary.ContainsKey(or.CarId) ? carMileageDictionary[or.CarId] : 0,
                    RemainingFuel = carRemainingFuelDictionary.ContainsKey(or.CarId) ? carMileageDictionary[or.CarId] : 0
                })
                .ToList();

            var accountIdStr = TempData["AccountId"] as string;
            TempData.Keep("AccountId");
            var mechanic = new MechanicDto();
            if (int.TryParse(accountIdStr, out int accountId))
            {
                var mechanicResponse = await _mechanicDataStore.GetMechanics(accountId);
                mechanic = mechanicResponse.Data.FirstOrDefault();
            }
            ViewBag.MechanicId = mechanic.Id;

            ViewBag.FilteredOperatorResponse = filteredOperatorResponse;
            ViewBag.CarData = carData;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IsAccepted,Comments,MechanicId,Distance,CarId,DriverId")] MechanicAcceptanceForCreateDto mechanicAcceptanceForCreateDto)
        {
            if (ModelState.IsValid)
            {
                var car = await _carDataStore.GetCarAsync(mechanicAcceptanceForCreateDto.CarId);
                if (car == null)
                {
                    ModelState.AddModelError(string.Empty, "Avtomobil mavjud emas !");
                }
                else
                {
                    if (mechanicAcceptanceForCreateDto.IsAccepted == null)
                    {
                        mechanicAcceptanceForCreateDto.IsAccepted = false;
                    }

                    mechanicAcceptanceForCreateDto.Date = DateTime.Now.ToTashkentTime();
                    mechanicAcceptanceForCreateDto.Status = mechanicAcceptanceForCreateDto.IsAccepted ? StatusForDto.Pending : StatusForDto.Rejected;
                    await _mechanicAcceptanceDataStore.CreateMechanicAcceptanceAsync(mechanicAcceptanceForCreateDto);
                    return RedirectToAction(nameof(PersonalIndex));
                }
            }

            var carData = await _carDataStore.GetCarsAsync(1, null);
            ViewBag.CarData = carData;

            var operatorResponse = await _operatorReviewDataStore.GetOperatorReviews(null, null, DateTime.Today.ToTashkentTime(), "Completed", 1, null);
            var mechanicAcceptanceResponse = await _mechanicAcceptanceDataStore.GetMechanicAcceptancesAsync(null, null, DateTime.Today.ToTashkentTime(), null, 10);

            var mechanicDriverIds = mechanicAcceptanceResponse.Data.Select(ma => ma.DriverId).ToHashSet();
            var filteredOperatorResponse = operatorResponse.Data.Where(or => !mechanicDriverIds.Contains(or.DriverId)).ToList();

            ViewBag.FilteredOperatorResponse = filteredOperatorResponse;

            return View("CreateByButton", mechanicAcceptanceForCreateDto);
        }



        public async Task<IActionResult> CreateByLink(int driverId, int carId, string carName, string driverName, double remainingFuel)
        {
            var accountIdStr = TempData["AccountId"] as string;
            TempData.Keep("AccountId");
            var mechanic = new MechanicDto();
            if (int.TryParse(accountIdStr, out int accountId))
            {
                var mechanicResponse = await _mechanicDataStore.GetMechanics(accountId);
                mechanic = mechanicResponse.Data.FirstOrDefault();
            }

            var car = await _carDataStore.GetCarAsync(carId);
            var mileage = car?.Mileage ?? 0;

            ViewBag.Mileage = mileage;
            ViewBag.MechanicId = mechanic.Id;
            ViewBag.CarId = carId;
            ViewBag.DriverId = driverId;
            ViewBag.CarName = carName;
            ViewBag.DriverName = driverName;
            ViewBag.RemainingFuel = remainingFuel;

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var review = await _mechanicAcceptanceDataStore.GetMechanicAcceptanceAsync(id);

            var drivers = await _driverDataStore.GetDriversAsync(1, null);
            var cars = await _carDataStore.GetCarsAsync(1, null);

            ViewBag.DriverSelectList = new SelectList(drivers.Data.Select(driver => new
            {
                Id = driver.Id,
                DisplayText = $"{driver.FirstName} {driver.LastName}"
            }), "Id", "DisplayText");

            ViewBag.CarSelectList = new SelectList(cars.Data.Select(car => new
            {
                Id = car.Id,
                DisplayText = $"{car.Model} ({car.Number})"
            }), "Id", "DisplayText");

            ViewBag.Status = Enum.GetValues(typeof(StatusForDto)).Cast<StatusForDto>().Select(e => new SelectListItem
            {
                Value = e.ToString(),
                Text = e switch
                {
                    StatusForDto.Pending => "Kutilmoqda",
                    StatusForDto.Completed => "Yakunlangan",
                    StatusForDto.Rejected => "Rad etilgan",
                    StatusForDto.Unassigned => "Tayinlanmagan",
                    StatusForDto.RejectedByDriver => "Haydovchi tomonidan rad etilgan",
                    _ => "No`malum holat"
                }
            }).ToList();
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MechanicAcceptanceForUpdateDto mechanicAcceptance)
        {
            if (id != mechanicAcceptance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _mechanicAcceptanceDataStore.UpdateMechanicAcceptanceAsync(id, mechanicAcceptance);
                }
                catch (Exception)
                {
                    if (!await MechanicAcceptanceExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(mechanicAcceptance);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var mechanicAcceptance = await _mechanicAcceptanceDataStore.GetMechanicAcceptanceAsync(id);
            if (mechanicAcceptance == null)
            {
                return NotFound();
            }
            return View(mechanicAcceptance);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mechanicAcceptanceDataStore.DeleteMechanicAcceptanceAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> MechanicAcceptanceExists(int id)
        {
            var mechanicAcceptance = await _mechanicAcceptanceDataStore.GetMechanicAcceptanceAsync(id);
            return mechanicAcceptance != null;
        }

        public async Task<IActionResult> Details(int id)
        {
            var mechanicAcceptence = await _mechanicAcceptanceDataStore.GetMechanicAcceptanceAsync(id);

            return View(mechanicAcceptence);
        }

        [HttpGet]
        public async Task<IActionResult> GetCarDetails(int carId)
        {
            var car = await _carDataStore.GetCarAsync(carId); 
            if (car != null)
            {
                var carDetails = new
                {
                    mileage = car.Mileage,
                    remainingFuel = car.RemainingFuel,
                };  
                return Json(carDetails);
            }
            return NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> Download(int year, int month)
        {
            var result = await _mechanicAcceptanceDataStore.GetExportFile(year, month);

            if (result == null || result.Length == 0)
            {
                return NotFound();
            }

            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Mexanik(Qabul Qilish) {month}.{year}.xlsx");
        }
    }
}