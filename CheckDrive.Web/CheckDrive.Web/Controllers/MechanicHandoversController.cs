using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.MechanicHandover;
using CheckDrive.Web.Extensions;
using CheckDrive.Web.Stores.Cars;
using CheckDrive.Web.Stores.DoctorReviews;
using CheckDrive.Web.Stores.Drivers;
using CheckDrive.Web.Stores.MechanicHandovers;
using CheckDrive.Web.Stores.Mechanics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckDrive.Web.Controllers
{
    public class MechanicHandoversController(IMechanicHandoverDataStore mechanicHandoverDataStore, IDriverDataStore driverDataStore, ICarDataStore carDataStore, IMechanicDataStore mechanicDataStore, IDoctorReviewDataStore doctorReviewDataStore) : Controller
    {
        private readonly IMechanicHandoverDataStore _mechanicHandoverDataStore = mechanicHandoverDataStore;
        private readonly IDriverDataStore _driverDataStore = driverDataStore;
        private readonly ICarDataStore _carDataStore = carDataStore;
        private readonly IMechanicDataStore _mechanicDataStore = mechanicDataStore;
        private readonly IDoctorReviewDataStore _doctorReviewDataStore = doctorReviewDataStore;

        public async Task<IActionResult> Index(int? pageNumber, string? searchString, DateTime? date)
        {
            var response = await _mechanicHandoverDataStore.GetMechanicHandoversAsync(pageNumber, searchString, date, null, 1);

            ViewBag.PageSize = response.PageSize;
            ViewBag.PageCount = response.TotalPages;
            ViewBag.TotalCount = response.TotalCount;
            ViewBag.CurrentPage = response.PageNumber;
            ViewBag.HasPreviousPage = response.HasPreviousPage;
            ViewBag.HasNextPage = response.HasNextPage;

            var mechanicHandovers = response.Data.Select(r => new
            {
                r.Id,
                IsHanded = (bool)r.IsHanded ? "Topshirildi" : "Topshirilmadi",
                r.Comments,
                Status = ((StatusForDto)r.Status) switch
                {
                    StatusForDto.Pending => "Kutilmoqda",
                    StatusForDto.Completed => "Yakunlangan",
                    StatusForDto.Rejected => "Rad etilgan",
                    StatusForDto.Unassigned => "Tayinlanmagan",
                    StatusForDto.RejectedByDriver => "Haydovchi tomonidan rad etilgan",
                    _ => "No`malum holat"
                },
                r.Date,
                r.Distance,
                r.DriverName,
                r.MechanicName,
                r.RemainingFuel,
                r.CarName,
                r.CarId
            }).ToList();

            ViewBag.MechanicHandovers = mechanicHandovers;

            return View();
        }

        public async Task<IActionResult> HistoryIndexForPersonalPage(int? pageNumber, string? searchString, DateTime? date)
        {
            var accountIdStr = TempData["AccountId"] as string;
            TempData.Keep("AccountId");
            int accountId = int.Parse(accountIdStr);

            var response = await _mechanicHandoverDataStore.GetMechanicHandoversAsync(pageNumber, searchString, date, accountId);

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
            var response = await _mechanicHandoverDataStore.GetMechanicHandoversAsync(pageNumber, searchString, null, null, 6);

            ViewBag.PageSize = response.PageSize;
            ViewBag.PageCount = response.TotalPages;
            ViewBag.TotalCount = response.TotalCount;
            ViewBag.CurrentPage = response.PageNumber;
            ViewBag.HasPreviousPage = response.HasPreviousPage;
            ViewBag.HasNextPage = response.HasNextPage;

            return View(response.Data);
        }

        public async Task<IActionResult> Create(int? driverId, string? driverName)
        {
            var accountIdStr = TempData["AccountId"] as string;
            TempData.Keep("AccountId");

            if (int.TryParse(accountIdStr, out int accountId))
            {
                var mechanicResponse = await _mechanicDataStore.GetMechanics(accountId);
                var mechanic = mechanicResponse.Data.First();
                if (mechanic != null)
                {
                    var response = await _mechanicHandoverDataStore.GetMechanicHandoversAsync(null, null, null, null, 6);
                    ViewBag.Drivers = response.Data
                        .Select(d => new SelectListItem
                        {
                            Value = d.DriverId.ToString(),
                            Text = d.DriverName,
                        })
                        .ToList();
                    var carResponse = await _carDataStore.GetCarsAsync(1, true);
                    ViewBag.Cars = carResponse.Data
                        .Select(c => new SelectListItem
                        {
                            Value = c.Id.ToString(),
                            Text = $"{c.Model} ({c.Number})"
                        })
                        .ToList();

                    ViewBag.SelectedDriverName = driverName;
                    ViewBag.SelectedDriverId = driverId;

                    return View(new MechanicHandoverForCreateDto { DriverId = driverId ?? 0, MechanicId = mechanic.Id });
                }
            }

            return NotFound("Механик не найден для указанного аккаунта.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IsHanded,Comments,MechanicId,Distance,RemainingFuel,CarId,DriverId")] MechanicHandoverForCreateDto mechanicHandoverForCreateDto)
        {
            if (ModelState.IsValid)
            {
                var car = await _carDataStore.GetCarAsync(mechanicHandoverForCreateDto.CarId);
                if (car != null && mechanicHandoverForCreateDto.Distance < car.Mileage)
                {
                    ModelState.AddModelError("Distance", "Masofa mashinaning mavjud yurgan masofasidan kam bo'lishi mumkin emas!");
                }

                if (ModelState.IsValid)
                {
                    if (mechanicHandoverForCreateDto.IsHanded == false)
                    {
                        mechanicHandoverForCreateDto.Status = StatusForDto.Rejected;
                    }

                    mechanicHandoverForCreateDto.Date = DateTime.Now.ToTashkentTime();
                    mechanicHandoverForCreateDto.Status = mechanicHandoverForCreateDto.IsHanded ? StatusForDto.Pending : StatusForDto.Rejected;
                    await _mechanicHandoverDataStore.CreateMechanicHandoverAsync(mechanicHandoverForCreateDto);
                    return RedirectToAction(nameof(PersonalIndex));
                }
            }

            var drivers = await GETDrivers();
            var cars = await GETCars();
            ViewBag.Drivers = new SelectList(drivers, "Value", "Text", mechanicHandoverForCreateDto.DriverId);
            ViewBag.Cars = new SelectList(cars, "Value", "Text");

            var selectedDriverName = drivers.FirstOrDefault(d => d.Value == mechanicHandoverForCreateDto.DriverId.ToString())?.Text;
            ViewBag.SelectedDriverName = selectedDriverName ?? string.Empty;
            ViewBag.SelectedDriverId = mechanicHandoverForCreateDto.DriverId;

            return View(mechanicHandoverForCreateDto);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var review = await _mechanicHandoverDataStore.GetMechanicHandoverAsync(id);

            if (review == null)
            {
                return NotFound();
            }

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

            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MechanicHandoverForUpdateDto mechanicHandover)
        {
            if (id != mechanicHandover.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _mechanicHandoverDataStore.UpdateMechanicHandoverAsync(id, mechanicHandover);
                }
                catch (Exception)
                {
                    if (!await MechanicHandoverExists(id))
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

            ViewBag.Drivers = new SelectList(await GETDrivers(), "Value", "Text");
            ViewBag.Cars = new SelectList(await GETCars(), "Value", "Text");

            return View(mechanicHandover);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var mechanicHandover = await _mechanicHandoverDataStore.GetMechanicHandoverAsync(id);
            if (mechanicHandover == null)
            {
                return NotFound();
            }
            return View(mechanicHandover);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mechanicHandoverDataStore.DeleteMechanicHandoverAsync(id);
            return RedirectToAction(nameof(Index));
        }
        private async Task<bool> MechanicHandoverExists(int id)
        {
            var mechanicAcceptance = await _mechanicHandoverDataStore.GetMechanicHandoverAsync(id);
            return mechanicAcceptance != null;
        }
        private async Task<List<SelectListItem>> GETCars()
        {
            var carResponse = await _carDataStore.GetCarsAsync(1, null);
            var cars = carResponse.Data
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Model} ({c.Number})"
                })
                .ToList();
            return cars;
        }
        private async Task<List<SelectListItem>> GETDrivers()
        {
            var driverResponse = await _driverDataStore.GetDriversAsync(1, null);
            var drivers = driverResponse.Data
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = $"{d.FirstName} {d.LastName}"
                })
                .ToList();
            return drivers;
        }

        public async Task<IActionResult> Details(int id)
        {
            var mechanicHandover = await _mechanicHandoverDataStore.GetMechanicHandoverAsync(id);

            return View(mechanicHandover);
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
                    remainingFuel = car.RemainingFuel
                };
                return Json(carDetails);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Download(int year, int month)
        {
            var result = await _mechanicHandoverDataStore.GetExportFile(year, month);

            if (result == null || result.Length == 0)
            {
                return NotFound();
            }

            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Mexanik(Topshiruvchih).xlsx");
        }
    }
}