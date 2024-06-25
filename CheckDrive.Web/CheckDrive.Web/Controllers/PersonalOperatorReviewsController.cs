using CheckDrive.ApiContracts;
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
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

namespace CheckDrive.Web.Controllers
{
    public class PersonalOperatorReviewsController(
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

        public async Task<IActionResult> Index(int? pageNumber, string? searchString)
        {
            var reviewsResponse = await _operatorReviewDataStore.GetOperatorReviews(null,null,null);
            var mechanicHandoverResponse = await _mechanicHandover.GetMechanicHandoversAsync(pageNumber,null,null);
            var cars = await _carDataStore.GetCarsAsync(null, null);

            var mechanicHandovers = mechanicHandoverResponse.Data
                .Where(m => m.Date.Value.Date == DateTime.Today)
                .Where(m => m.IsHanded == true)
                .ToList();

            var operators = new List<OperatorReviewDto>();

            foreach (var mechanicHandover in mechanicHandovers)
            {
                var car = cars.Data.FirstOrDefault(c => c.Id == mechanicHandover.CarId);
                var review = reviewsResponse.Data.FirstOrDefault(r => r.DriverId == mechanicHandover.DriverId);
                if (review != null)
                {
                    if (review.Date.Value.Date == DateTime.Today)
                    {
                        operators.Add(new OperatorReviewDto
                        {
                            DriverId = review.DriverId,
                            DriverName = mechanicHandover.DriverName,
                            OperatorName = review.OperatorName,
                            CarId = car?.Id ?? review.CarId,
                            CarModel = car?.Model ?? review.CarModel,
                            CarNumber = car?.Number ?? review.CarNumber,
                            CarOilCapacity = car?.FuelTankCapacity.ToString() ?? review.CarOilCapacity,
                            CarOilRemainig = car?.RemainingFuel.ToString() ?? review.CarOilRemainig,
                            OilAmount = review.OilAmount,
                            OilMarks = review.OilMarks,
                            IsGiven = review.IsGiven,
                            Comments = review.Comments,
                            Date = review.Date,
                            Status = review.Status
                        });
                    }
                    else
                    {
                        operators.Add(new OperatorReviewDto
                        {
                            DriverId = mechanicHandover.DriverId,
                            DriverName = mechanicHandover.DriverName,
                            OperatorName = null,
                            CarId = car?.Id ?? 0,
                            CarModel = car?.Model ?? string.Empty,
                            CarNumber = car?.Number ?? string.Empty,
                            CarOilCapacity = car?.FuelTankCapacity.ToString() ?? string.Empty,
                            CarOilRemainig = car?.RemainingFuel.ToString() ?? string.Empty,
                            OilAmount = null,
                            OilMarks = null,
                            IsGiven = null,
                            Comments = null,
                            Date = null,
                            Status = StatusForDto.Unassigned
                        });
                    }
                }
                else
                {
                    operators.Add(new OperatorReviewDto
                    {
                        DriverId = mechanicHandover.DriverId,
                        DriverName = mechanicHandover.DriverName,
                        OperatorName = null,
                        CarId = car?.Id ?? 0,
                        CarModel = car?.Model ?? string.Empty,
                        CarNumber = car?.Number ?? string.Empty,
                        CarOilCapacity = car?.FuelTankCapacity.ToString() ?? string.Empty,
                        CarOilRemainig = car?.RemainingFuel.ToString() ?? string.Empty,
                        OilAmount = null,
                        OilMarks = null,
                        IsGiven = null,
                        Comments = null,
                        Date = null,
                        Status = StatusForDto.Unassigned
                    }); 
                }
            }

            return View(operators);
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
                operatorr = operatorResponse.Data.First();
            }
            var operators = new List<SelectListItem>
        {
            new SelectListItem { Value = operatorr.Id.ToString(), Text = $"{operatorr.FirstName} {operatorr.LastName}" }
        };

            var response = await _operatorReviewDataStore.GetOperatorReviews(null, null, null);
            var oilMarks = response.Data.Select(r => r.OilMarks).Distinct().ToList();
            var mechanicHandovers = await _mechanicHandover.GetMechanicHandoversAsync();

            var healthyDrivers = mechanicHandovers.Data
                                  .Where(dr => dr.IsHanded == true && dr.Date.Value.Date == DateTime.Today)
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
            ViewBag.Cars = new SelectList(cars, "Value", "Text", carId);

            var model = new OperatorReviewForCreateDto();

            if (driverId.HasValue)
            {
                model.DriverId = driverId.Value;
                ViewBag.SelectedDriverName = driverName;
                ViewBag.DriverId = driverId.Value; 
            }

            if (carId.HasValue)
            {
                model.CarId = carId.Value;
                ViewBag.SelectedCar = $"{carModel} Sig`imi: {fuelTankCapacity?.ToString() ?? "N/A"} litr, Qoldig`i: {remainingFuel?.ToString() ?? "N/A"} litr";
            }

            if (fuelTankCapacity.HasValue && remainingFuel.HasValue)
            {
                ViewBag.FuelTankCapacity = fuelTankCapacity.Value;
                ViewBag.RemainingFuel = remainingFuel.Value;
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OilAmount,Comments,Status,Date,OperatorId,DriverId,CarId,OilMarks,IsGiven")] OperatorReviewForCreateDto operatorReview)
        {
            if (ModelState.IsValid)
            {
                operatorReview.Date = DateTime.Now;

                double maxOilAmount = await GetMaxOilAmount(operatorReview.CarId);

                if ((operatorReview.OilAmount == 0 && operatorReview.IsGiven == true) ||
                    (operatorReview.OilAmount > 0 && operatorReview.IsGiven == false))
                {
                    ModelState.AddModelError("IsGiven", "Siz yoqilg`ini 0 litr berib, berdim tugmasini bostingiz yoki\nYoqilg`ini berib bermadim tugmasini bostingiz");
                }
                else if (operatorReview.OilAmount < 0 || operatorReview.OilAmount > maxOilAmount)
                {
                    ModelState.AddModelError("OilAmount", $"Oil amount must be between 0 and {maxOilAmount}.");
                }
                else
                {
                    await _operatorReviewDataStore.CreateOperatorReview(operatorReview);
                    return RedirectToAction(nameof(Index));
                }
            }

            var drivers = await GETDrivers();
            var cars = await GETCars();
            var response = await _operatorReviewDataStore.GetOperatorReviews(null, null, null);
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
    }
}
