using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.Car;
using CheckDrive.ApiContracts.Mechanic;
using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.Web.Stores.Cars;
using CheckDrive.Web.Stores.Drivers;
using CheckDrive.Web.Stores.MechanicAcceptances;
using CheckDrive.Web.Stores.MechanicHandovers;
using CheckDrive.Web.Stores.Mechanics;
using CheckDrive.Web.Stores.OperatorReviews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckDrive.Web.Controllers
{
    public class MechanicAcceptancesController : Controller
    {
        private readonly IMechanicAcceptanceDataStore _mechanicAcceptanceDataStore;
        private readonly IDriverDataStore _driverDataStore;
        private readonly ICarDataStore _carDataStore;
        private readonly IMechanicDataStore _mechanicDataStore;
        private readonly IOperatorReviewDataStore _operatorReviewDataStore;
        private readonly IMechanicHandoverDataStore _mechanicHandoverDataStore;

        public MechanicAcceptancesController(IMechanicAcceptanceDataStore mechanicAcceptanceDataStore, IDriverDataStore driverDataStore, ICarDataStore carDataStore, IMechanicDataStore mechanicDataStore, IOperatorReviewDataStore operatorReviewDataStore, IMechanicHandoverDataStore mechanicHandoverDataStore)
        {
            _mechanicAcceptanceDataStore = mechanicAcceptanceDataStore;
            _driverDataStore = driverDataStore;
            _carDataStore = carDataStore;
            _mechanicDataStore = mechanicDataStore;
            _operatorReviewDataStore = operatorReviewDataStore;
            _mechanicHandoverDataStore = mechanicHandoverDataStore;
        }

        public async Task<IActionResult> Index(int? pageNumber, string? searchString, DateTime? date)
        {

            var response = await _mechanicAcceptanceDataStore.GetMechanicAcceptancesAsync(pageNumber, searchString, date, 1);

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
                Status = ((StatusForDto)r.Status) switch
                {
                    StatusForDto.Pending => "Pending",
                    StatusForDto.Completed => "Completed",
                    StatusForDto.Rejected => "Rejected",
                    StatusForDto.Unassigned => "Unassigned",
                    _ => "Unknown Status"
                },
                r.Date,
                r.Distance,
                r.DriverName,
                r.MechanicName,
                r.CarName,
                r.CarId
            }).ToList();

            ViewBag.MechanicAcceptances = mechanicAcceptances;

            return View();
        }


        public async Task<IActionResult> PersonalIndex(string? searchString, int? pageNumber)
        {
            var response = await _mechanicAcceptanceDataStore.GetMechanicAcceptancesAsync(pageNumber, searchString, null, 6);

            ViewBag.PageSize = response.PageSize;
            ViewBag.PageCount = response.TotalPages;
            ViewBag.TotalCount = response.TotalCount;
            ViewBag.CurrentPage = response.PageNumber;
            ViewBag.HasPreviousPage = response.HasPreviousPage;
            ViewBag.HasNextPage = response.HasNextPage;

            return View(response.Data);
        }
        public async Task<IActionResult> Create(int? driverId, string? driverName, int? carId, string? carModel)
        {
            var drivers = await GETDrivers();
            var cars = await GETCars();

            var mechanic = new MechanicDto();

            var accountIdStr = TempData["AccountId"] as string;
            TempData.Keep("AccountId");
            if (int.TryParse(accountIdStr, out int accountId))
            {
                var mechanicResponse = await _mechanicDataStore.GetMechanics(accountId);
                mechanic = mechanicResponse.Data.FirstOrDefault();
            }
            var mechanics = new List<SelectListItem>
            {
                new SelectListItem { Value = mechanic.Id.ToString(), Text = $"{mechanic.FirstName} {mechanic.LastName}" }
            };

            var response = await _mechanicAcceptanceDataStore.GetMechanicAcceptancesAsync(null, null, null, 6);
            var operatorReviews = await _operatorReviewDataStore.GetOperatorReviews(null, null, null, null);

            var healthyDrivers = operatorReviews.Data
                                  .Where(dr => dr.IsGiven == true && dr.Date.Value.Date == DateTime.Today)
                                  .Select(dr => dr.DriverId)
                                  .ToList();

            var acceptedDrivers = response.Data
                .Where(ma => ma.Date.HasValue && ma.Date.Value.Date == DateTime.Today)
                .Select(ma => ma.DriverId)
                .ToList();

            var filteredDrivers = drivers
                .Where(d => healthyDrivers.Contains(int.Parse(d.Value)) && !acceptedDrivers.Contains(int.Parse(d.Value)))
                .ToList();

            ViewBag.Drivers = new SelectList(filteredDrivers, "Value", "Text");
            ViewBag.Mechanics = mechanics;

            if (!driverId.HasValue && !carId.HasValue && filteredDrivers.Any())
            {
                var firstDriverId = int.Parse(filteredDrivers.First().Value);
                var operatorReview = operatorReviews.Data.FirstOrDefault(m => m.DriverId == firstDriverId && m.Date.Value.Date == DateTime.Today);

                if (operatorReview != null)
                {
                    carId = operatorReview.CarId;
                    var car = await _carDataStore.GetCarAsync(operatorReview.CarId);
                    carModel = car?.Model;
                }

                driverId = firstDriverId;
            }

            ViewBag.Cars = new SelectList(cars, "Value", "Text", carId);

            var model = new MechanicAcceptanceForCreateDto();

            if (driverId.HasValue)
            {
                model.DriverId = driverId.Value;
                ViewBag.SelectedDriverName = driverName;
                ViewBag.DriverId = driverId.Value;

                if (carId.HasValue)
                {
                    model.CarId = carId.Value;
                    ViewBag.SelectedCar = $"{carModel}";
                }
            }

            return View(model);
        }

        public async Task<IActionResult> GetCarByDriverId(int driverId)
        {
            var operatorReviews = await _operatorReviewDataStore.GetOperatorReviews(null, null, null, null);
            var operatorr = operatorReviews.Data.FirstOrDefault(m => m.DriverId == driverId && m.Date.HasValue && m.Date.Value.Date == DateTime.Today);

            if (operatorr != null)
            {
                var carId = operatorr.CarId;
                var car = await _carDataStore.GetCarAsync(carId);

                if (car != null)
                {
                    return Json(new { success = true, car });
                }
            }

            return Json(new { success = false });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IsAccepted,Comments,MechanicId,Distance,DriverId,CarId")] MechanicAcceptanceForCreateDto acceptance)
        {

            if (ModelState.IsValid)
            {
                acceptance.Date = DateTime.Now;
                var car = _carDataStore.GetCarAsync(acceptance.CarId);
                var carr = new CarForUpdateDto
                {
                    Id = acceptance.CarId,
                    Color = car.Result.Color,
                    Model = car.Result.Model,
                    Number = car.Result.Number,
                };


                if ((acceptance.Distance < 0 && acceptance.IsAccepted == true) ||
                    (acceptance.Distance > 0 && acceptance.IsAccepted == false))
                {
                    ModelState.AddModelError("IsAccepted", "Qabul qilish masofasini kiritmadingiz");
                }
                else
                {
                    await _carDataStore.UpdateCarAsync(acceptance.CarId, carr);
                    await _mechanicAcceptanceDataStore.CreateMechanicAcceptanceAsync(acceptance);
                    return RedirectToAction(nameof(PersonalIndex));
                }
            }

            return View(acceptance);
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

            ViewBag.Drivers = new SelectList(await GETDrivers(), "Value", "Text");
            ViewBag.Cars = new SelectList(await GETCars(), "Value", "Text");

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

        private async Task<List<SelectListItem>> GETMechanics()
        {
            var mechanicResponse = await _mechanicDataStore.GetMechanicsAsync();
            var mechanics = mechanicResponse.Data
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = $"{d.FirstName} {d.LastName}"
                })
                .ToList();
            return mechanics;
        }
        private async Task<List<SelectListItem>> GETCars()
        {
            var carResponse = await _carDataStore.GetCarsAsync(null, null);
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
            var driverResponse = await _driverDataStore.GetDriversAsync(null, null);
            var drivers = driverResponse.Data
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = $"{d.FirstName} {d.LastName}"
                })
                .ToList();
            return drivers;
        }
    }
}