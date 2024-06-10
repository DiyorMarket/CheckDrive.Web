using CheckDrive.ApiContracts;
using CheckDrive.Web.Models;
using CheckDrive.Web.Stores.Cars;
using CheckDrive.Web.Stores.Drivers;
using CheckDrive.Web.Stores.OperatorReviews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckDrive.Web.Controllers
{
    public class PersonalOperatorReviewsController(IOperatorReviewDataStore operatorReviewDataStore, IDriverDataStore driverDataStore, ICarDataStore carDataStore) : Controller
    {
        private readonly IOperatorReviewDataStore _operatorReviewDataStore = operatorReviewDataStore;
        private readonly IDriverDataStore _driverDataStore = driverDataStore;
        private readonly ICarDataStore _carDataStore = carDataStore;

        public async Task<IActionResult> Index()
        {
            var response = await _operatorReviewDataStore.GetOperatorReviews();

            var operatorReviews = response.Data.Select(r => new
            {
                r.Id,
                r.DriverName,
                r.CarModel,
                r.CarNumber,
                r.CarOilCapacity,
                r.CarOilRemainig,
                r.OilAmount,
                OilMarks = ((OilMarksForDto)r.OilMarks) switch
                {
                    OilMarksForDto.A80 => "A80",
                    OilMarksForDto.A91 => "A91",
                    OilMarksForDto.A92 => "A92",
                    OilMarksForDto.A95 => "A95",
                    _ => "Unknown Status"
                },
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
                r.CarId
            }).ToList();

            ViewBag.OperatorReviews = operatorReviews;
            return View();
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

        public async Task<IActionResult> Create()
        {
            var drivers = await GETDrivers();
            var cars = await GETCars();
            var response = await _operatorReviewDataStore.GetOperatorReviews();
            var oilMarks = response.Data.Select(r => new { r.OilMarks });

            ViewBag.OilMarks = oilMarks;
            ViewBag.Drivers = new SelectList(drivers, "Value", "Text");
            ViewBag.Cars = new SelectList(cars, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OilAmount,Comments,Status,Date,OperatorId,DriverId")] OperatorReview operatorReview)
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
            var driverResponse = await _driverDataStore.GetDriversAsync(null);
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
