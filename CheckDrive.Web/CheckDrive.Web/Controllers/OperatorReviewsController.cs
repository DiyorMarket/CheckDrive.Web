using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.Car;
using CheckDrive.ApiContracts.Operator;
using CheckDrive.ApiContracts.OperatorReview;
using CheckDrive.Web.Models;
using CheckDrive.Web.Stores.Cars;
using CheckDrive.Web.Stores.Drivers;
using CheckDrive.Web.Stores.MechanicHandovers;
using CheckDrive.Web.Stores.OperatorReviews;
using CheckDrive.Web.Stores.Operators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckDrive.Web.Controllers
{
    public class OperatorReviewsController(
        IOperatorReviewDataStore operatorReviewDataStore,
        IMechanicHandoverDataStore mechanicHandover,
        ICarDataStore carDataStore,
        IDriverDataStore driverDataStore,
        IOperatorDataStore operatorDataStore) : Controller
    {
        private readonly IOperatorReviewDataStore _operatorReviewDataStore = operatorReviewDataStore;
        private readonly IMechanicHandoverDataStore _mechanicHandover = mechanicHandover;
        private readonly ICarDataStore _carDataStore = carDataStore;
        private readonly IDriverDataStore _driverDataStore = driverDataStore;
        private readonly IOperatorDataStore _operatorDataStore = operatorDataStore;

        public async Task<IActionResult> Index(int? pageNumber, string? searchString, DateTime? date)
        {

            var operatorReviews = await _operatorReviewDataStore.GetOperatorReviews(pageNumber, searchString, date, 1);

            ViewBag.PageSize = operatorReviews.PageSize;
            ViewBag.PageCount = operatorReviews.TotalPages;
            ViewBag.TotalCount = operatorReviews.TotalCount;
            ViewBag.CurrentPage = operatorReviews.PageNumber;
            ViewBag.HasPreviousPage = operatorReviews.HasPreviousPage;
            ViewBag.HasNextPage = operatorReviews.HasNextPage;

            var operatorReviewss = operatorReviews.Data.Select(r => new
            {
                r.Id,
                r.OperatorName,
                r.DriverName,
                r.OilAmount,
                r.Date,
                r.Comments,
                Status = ((StatusForDto)r.Status) switch
                {
                    StatusForDto.Pending => "Pending",
                    StatusForDto.Completed => "Completed",
                    StatusForDto.Rejected => "Rejected",
                    StatusForDto.Unassigned => "Unassigned",
                    _ => "Unknown Status"
                }
            }).ToList();

            ViewBag.OperatorsReview = operatorReviewss;
            return View();

        }

        public async Task<IActionResult> PersonalIndex(int? pageNumber, string? searchString)
        {
            var reviewsResponse = await _operatorReviewDataStore.GetOperatorReviews(pageNumber, searchString, null, 4);

            return View(reviewsResponse.Data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var operatorReview = await _operatorReviewDataStore.GetOperatorReview(id);
            if (operatorReview == null)
            {
                return NotFound();
            }
            return View(operatorReview);
        }

        public async Task<IActionResult> Create(int? driverId, string? driverName, int? carId, string? carModel, double? fuelTankCapacity, double? remainingFuel)
        {
            var drivers = await GETDrivers();
            var cars = await GETCars();

            var operatorr = new OperatorDto();

            var accountIdStr = TempData["AccountId"] as string;
            TempData.Keep("AccountId");
            if (int.TryParse(accountIdStr, out int accountId))
            {
                var operatorResponse = await _operatorDataStore.GetOperators(accountId);
                operatorr = operatorResponse.Data.FirstOrDefault();
            }
            var operators = new List<SelectListItem>
            {
                new SelectListItem { Value = operatorr.Id.ToString(), Text = $"{operatorr.FirstName} {operatorr.LastName}" }
            };

            var response = await _operatorReviewDataStore.GetOperatorReviews(null, null, null, 1);
            var oilMarks = GetOilMarks();
            var mechanicHandovers = await _mechanicHandover.GetMechanicHandoversAsync();

            var healthyDrivers = mechanicHandovers.Data
                                  .Where(dr => dr.IsHanded == true && dr.Date.Date == DateTime.Today)
                                  .Select(dr => dr.DriverId)
                                  .ToList();

            var givedDrivers = response.Data
                .Where(ma => ma.Date.HasValue && ma.Date.Value.Date == DateTime.Today)
                .Select(ma => ma.DriverId)
                .ToList();

            var filteredDrivers = drivers
                .Where(d => healthyDrivers.Contains(int.Parse(d.Value)) && !givedDrivers.Contains(int.Parse(d.Value)))
                .ToList();

            ViewBag.OilMarks = new SelectList(oilMarks);
            ViewBag.Drivers = new SelectList(filteredDrivers, "Value", "Text");
            ViewBag.Operators = operators;

            if (!driverId.HasValue && !carId.HasValue && filteredDrivers.Any())
            {
                var firstDriverId = int.Parse(filteredDrivers.First().Value);
                var mechanicHandover = mechanicHandovers.Data.FirstOrDefault(m => m.DriverId == firstDriverId && m.Date.Date == DateTime.Today);

                if (mechanicHandover != null)
                {
                    carId = mechanicHandover.CarId;
                    var car = await _carDataStore.GetCarAsync(mechanicHandover.CarId);
                    carModel = car?.Model;
                    fuelTankCapacity = car?.FuelTankCapacity;
                    remainingFuel = car?.RemainingFuel;
                }

                driverId = firstDriverId;
            }

            ViewBag.Cars = new SelectList(cars, "Value", "Text", carId);

            var model = new OperatorReviewForCreateDto();

            if (driverId.HasValue)
            {
                model.DriverId = driverId.Value;
                ViewBag.SelectedDriverName = driverName;
                ViewBag.DriverId = driverId.Value;

                if (carId.HasValue)
                {
                    model.CarId = carId.Value;
                    ViewBag.SelectedCar = $"{carModel} Sig`imi: {fuelTankCapacity?.ToString() ?? "N/A"} litr, Qoldig`i: {remainingFuel?.ToString() ?? "N/A"} litr";
                }
            }

            if (fuelTankCapacity.HasValue && remainingFuel.HasValue)
            {
                ViewBag.FuelTankCapacity = fuelTankCapacity.Value;
                ViewBag.RemainingFuel = remainingFuel.Value;
            }

            return View(model);
        }

        public async Task<IActionResult> GetCarByDriverId(int driverId)
        {
            var mechanicHandovers = await _mechanicHandover.GetMechanicHandoversAsync();
            var handover = mechanicHandovers.Data.FirstOrDefault(m => m.DriverId == driverId && m.Date.Date == DateTime.Today);

            if (handover != null)
            {
                var car = await _carDataStore.GetCarAsync(handover.CarId);
                return Json(new { success = true, car });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OilAmount,Comments,Status,Date,OperatorId,DriverId,CarId,OilMarks,IsGiven")] OperatorReviewForCreateDto operatorReview)
        {
            var car = _carDataStore.GetCarAsync(operatorReview.CarId);
            var carr = new CarForUpdateDto
            {
                Id = operatorReview.CarId,
                Color = car.Result.Color,
                FuelTankCapacity = car.Result.FuelTankCapacity,
                ManufacturedYear = car.Result.ManufacturedYear,
                MeduimFuelConsumption = car.Result.MeduimFuelConsumption,
                Model = car.Result.Model,
                Number = car.Result.Number,
                RemainingFuel = car.Result.RemainingFuel + operatorReview.OilAmount,
            };

            if (ModelState.IsValid)
            {
                operatorReview.Date = DateTime.Now;

                await _carDataStore.UpdateCarAsync(operatorReview.CarId, carr);

                double maxOilAmount = await GetMaxOilAmount(operatorReview.CarId);

                if ((operatorReview.OilAmount < 0 && operatorReview.IsGiven == true) ||
                    (operatorReview.OilAmount > 0 && operatorReview.IsGiven == false))
                {
                    ModelState.AddModelError("IsGiven", "Yoqilg`ini berib bermadim tugmasini bostingiz");
                }
                else if (operatorReview.OilAmount < 0 || operatorReview.OilAmount > maxOilAmount)
                {
                    ModelState.AddModelError("OilAmount", $"Oil amount must be between 0 and {maxOilAmount}.");
                }
                else
                {
                    await _operatorReviewDataStore.CreateOperatorReview(operatorReview);
                    return RedirectToAction(nameof(PersonalIndex));
                }
            }

            var drivers = await GETDrivers();
            var cars = await GETCars();
            var response = await _operatorReviewDataStore.GetOperatorReviews(null, null, null, 1);
            var oilMarks = response.Data.Select(r => r.OilMarks).Distinct().ToList();

            ViewBag.OilMarks = new SelectList(oilMarks);
            ViewBag.Drivers = new SelectList(drivers, "Value", "Text");
            ViewBag.Cars = new SelectList(cars, "Value", "Text");

            return View(operatorReview);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var operatorReview = await _operatorReviewDataStore.GetOperatorReview(id);
            if (operatorReview == null)
            {
                return NotFound();
            }
            return View(operatorReview);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OilAmount,Comments,Status,Date,OperatorId,DriverId")] OperatorReview operatorReview)
        {
            if (id != operatorReview.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _operatorReviewDataStore.UpdateOperatorReview(id, operatorReview);
                }
                catch (Exception)
                {
                    if (!await OperatorReviewExists(id))
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
            return View(operatorReview);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var operatorReview = await _operatorReviewDataStore.GetOperatorReview(id);
            if (operatorReview == null)
            {
                return NotFound();
            }
            return View(operatorReview);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _operatorReviewDataStore.DeleteOperatorReview(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> OperatorReviewExists(int id)
        {
            var operatorReview = await _operatorReviewDataStore.GetOperatorReview(id);
            return operatorReview != null;
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

        private async Task<List<SelectListItem>> GETOperators()
        {
            var operatorResponse = await _operatorDataStore.GetOperators();
            var operators = operatorResponse.Data
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = $"{d.FirstName} {d.LastName}"
                })
                .ToList();
            return operators;
        }

        private async Task<List<SelectListItem>> GETCars()
        {
            var carResponse = await _carDataStore.GetCarsAsync(null, null);
            var cars = carResponse.Data
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = $"{d.Model} Sig`imi: {d.FuelTankCapacity} litr, Qoldig`i: {d.RemainingFuel} litr"
                })
                .ToList();
            return cars;
        }

        private async Task<double> GetMaxOilAmount(int carId)
        {
            var car = await _carDataStore.GetCarAsync(carId);
            return car.FuelTankCapacity - car.RemainingFuel;
        }

        private List<string> GetOilMarks()
        {
            return Enum.GetValues(typeof(OilMarksForDto))
                       .Cast<OilMarksForDto>()
                       .Select(o => o.ToString())
                       .ToList();
        }
    }
}
