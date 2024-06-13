using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.OperatorReview;
using CheckDrive.Web.Models;
using CheckDrive.Web.Stores.Cars;
using CheckDrive.Web.Stores.Drivers;
using CheckDrive.Web.Stores.MechanicAcceptances;
using CheckDrive.Web.Stores.OperatorReviews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckDrive.Web.Controllers
{
    public class PersonalOperatorReviewsController(
        IOperatorReviewDataStore operatorReviewDataStore, 
        IMechanicAcceptanceDataStore mechanicAcceptance, 
        ICarDataStore carDataStore, 
        IDriverDataStore driverDataStore) : Controller
    {
        private readonly IOperatorReviewDataStore _operatorReviewDataStore = operatorReviewDataStore;
        private readonly IMechanicAcceptanceDataStore _mechanicAcceptance = mechanicAcceptance;
        private readonly ICarDataStore _carDataStore = carDataStore;
        private readonly IDriverDataStore _driverDataStore = driverDataStore;

        public async Task<IActionResult> Index(int? pageNumber, string? searchString)
        {
            var reviewsResponse = await _operatorReviewDataStore.GetOperatorReviews();
            var mechanicHandoverResponse = await _mechanicAcceptance.GetMechanicAcceptancesAsync(pageNumber);

            var mechanicHandovers = mechanicHandoverResponse.Data
                .Where(m => m.Date.Value.Date == DateTime.Today)
                .Where(m => m.IsAccepted == true)
                .ToList();

            var operators = new List<OperatorReviewDto>();

            foreach (var mechanicHandover in mechanicHandovers)
            {
                var review = reviewsResponse.Data.FirstOrDefault(r => r.DriverId == mechanicHandover.DriverId);
                if (review != null)
                {
                    if (review.Date.Value.Date == DateTime.Today)
                    {
                        operators.Add(new OperatorReviewDto
                        {
                            DriverId = review.DriverId,
                            DriverName = mechanicHandover.DriverName,
                            CarModel = review.CarModel,
                            CarNumber = review.CarNumber,
                            CarOilCapacity = review.CarOilCapacity,
                            CarOilRemainig = review.CarOilRemainig,
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
                            CarModel = mechanicHandover.CarName,
                            CarNumber = string.Empty,
                            CarOilCapacity = string.Empty,
                            CarOilRemainig = string.Empty,
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
                        CarModel = string.Empty,
                        CarNumber = string.Empty,
                        CarOilCapacity = string.Empty,
                        CarOilRemainig = string.Empty,
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

        public async Task<IActionResult> Create(int driverId, string driverName)
        {
            var cars = await GETCars();
            var response = await _operatorReviewDataStore.GetOperatorReviews();
            var oilMarks = response.Data.Select(r => new { r.OilMarks });

            ViewBag.OilMarks = oilMarks;
            ViewBag.Cars = new SelectList(cars, "Value", "Text");
            ViewBag.SelectedDriverName = driverName;
            ViewBag.SelectedDriverId = driverId;

            return View(new OperatorReviewForCreateDto { DriverId = driverId, Date = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OilAmount,Comments,Status,Date,OperatorId,DriverId")] OperatorReviewForCreateDto operatorReview)
        {
            if (ModelState.IsValid)
            {
                await _operatorReviewDataStore.CreateOperatorReview(operatorReview);
                return RedirectToAction(nameof(Index));
            }
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
            var driverResponse = await _driverDataStore.GetDriversAsync(null,null);
            var drivers = driverResponse.Data
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = $"{d.FirstName} {d.LastName}"
                })
                .ToList();
            return drivers;
        }

        private async Task<List<SelectListItem>> GETCars()
        {
            var carResponse = await _carDataStore.GetCarsAsync(null, null);
            var cars = carResponse.Data
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = $"{d.Model}, Moshina raqami: {d.Number}, Yoqilg`i sig`imi: {d.FuelTankCapacity}, Yoqilg`i qoldig`i: {d.RemainingFuel}"
                })
                .ToList();
            return cars;
        }
    }
}
