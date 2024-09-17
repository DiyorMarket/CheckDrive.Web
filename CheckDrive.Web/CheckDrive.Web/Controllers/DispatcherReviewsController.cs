using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.Car;
using CheckDrive.ApiContracts.Dispatcher;
using CheckDrive.ApiContracts.DispatcherReview;
using CheckDrive.Web.Extensions;
using CheckDrive.Web.Stores.Cars;
using CheckDrive.Web.Stores.DispatcherReviews;
using CheckDrive.Web.Stores.Dispatchers;
using CheckDrive.Web.Stores.Drivers;
using CheckDrive.Web.Stores.MechanicAcceptances;
using CheckDrive.Web.Stores.MechanicHandovers;
using CheckDrive.Web.Stores.OperatorReviews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace CheckDrive.Web.Controllers
{
    public class DispatcherReviewsController : Controller
    {
        private readonly IDispatcherReviewDataStore _dispatcherReviewDataStore;
        private readonly IMechanicAcceptanceDataStore _mechanicAcceptanceDataStore;
        private readonly IMechanicHandoverDataStore _mechanicHandoverDataStore;
        private readonly IOperatorReviewDataStore _operatorDataStore;
        private readonly IDispatcherDataStore _dispatcherDataStore;
        private readonly IDriverDataStore _driverDataStore;
        private readonly ICarDataStore _carDataStore;

        public DispatcherReviewsController(
            IDispatcherReviewDataStore dispatcherReviewDataStore,
            IMechanicAcceptanceDataStore mechanicAcceptanceDataStore,
            IOperatorReviewDataStore operatorDataStore,
            IMechanicHandoverDataStore mechanicHandoverDataStore,
            IDispatcherDataStore dispatcherDataStore,
            IDriverDataStore driverDataStore,
            ICarDataStore carDataStore)
        {
            _dispatcherReviewDataStore = dispatcherReviewDataStore;
            _mechanicAcceptanceDataStore = mechanicAcceptanceDataStore;
            _operatorDataStore = operatorDataStore;
            _mechanicHandoverDataStore = mechanicHandoverDataStore;
            _driverDataStore = driverDataStore;
            _carDataStore = carDataStore;
            _dispatcherDataStore = dispatcherDataStore;
        }

        public async Task<IActionResult> Index(int? pagenumber, string? searchString, DateTime? date)
        {
            var response = await _dispatcherReviewDataStore.GetDispatcherReviews(pagenumber, searchString, date, "Completed");


            if (response is null)
            {
                return BadRequest();
            }
            ViewBag.PageSize = response.PageSize;
            ViewBag.PageCount = response.TotalPages;
            ViewBag.TotalCount = response.TotalCount;
            ViewBag.CurrentPage = response.PageNumber;
            ViewBag.HasPreviousPage = response.HasPreviousPage;
            ViewBag.HasNextPage = response.HasNextPage;

            var dispatcherReviewResponse = response.Data.Select(r => new
            {
                r.Id,
                FuelSpended = r.FuelSpended.ToString("0.00").PadLeft(4, '0'),
                RemainigFuelBefore = r.RemainigFuelBefore.ToString("0.00").PadLeft(4, '0'),
                RemainigFuelAfter = r.RemainigFuelAfter.ToString("0.00").PadLeft(4, '0'),
                r.PouredFuel,
                r.DistanceCovered,
                r.Date,
                r.CarMeduimFuelConsumption,
                r.CarName,
                r.DispatcherName,
                r.MechanicName,
                r.OperatorName,
                r.DriverName
            }).ToList();

            ViewBag.DispatcherReviews = dispatcherReviewResponse;
            return View();
        }

        public async Task<IActionResult> BorrowIndex(int? pagenumber, string? searchString, DateTime? date)
        {
            var response = await _dispatcherReviewDataStore.GetDispatcherReviews(pagenumber, searchString, date, "Rejected");


            if (response is null)
            {
                return BadRequest();
            }
            ViewBag.PageSize = response.PageSize;
            ViewBag.PageCount = response.TotalPages;
            ViewBag.TotalCount = response.TotalCount;
            ViewBag.CurrentPage = response.PageNumber;
            ViewBag.HasPreviousPage = response.HasPreviousPage;
            ViewBag.HasNextPage = response.HasNextPage;

            var dispatcherReviewResponse = response.Data.Select(r => new
            {
                r.Id,
                ChangedFuelSpended = (r.ChangedFuelSpendede ?? 0).ToString("0.00").PadLeft(4, '0'),
                FuelSpended = r.FuelSpended.ToString("0.00").PadLeft(4, '0'),
                RemainigFuelBefore = r.RemainigFuelBefore.ToString("0.00").PadLeft(4, '0'),
                RemainigFuelAfter = r.RemainigFuelAfter.ToString("0.00").PadLeft(4, '0'),
                r.PouredFuel,
                r.DistanceCovered,
                r.ChangedDistanceCovered,
                r.Date,
                r.CarMeduimFuelConsumption,
                r.CarName,
                r.DispatcherName,
                r.MechanicName,
                r.OperatorName,
                r.DriverName
            }).ToList();

            ViewBag.DispatcherReviews = dispatcherReviewResponse;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SaveReview(int id)
        {
            var review = await _dispatcherReviewDataStore.GetDispatcherReview(id);

            if (review == null)
            {
                return NotFound();
            }

            var reviewForUpdate = new DispatcherReviewForUpdateDto
            {
                Id = review.Id,
                FuelSpended = review.FuelSpended,
                DistanceCovered = review.DistanceCovered,
                Status = StatusForDto.Completed, 
                Comment = review.Comment,
                Date = review.Date,
                DispatcherId = review.DispatcherId,
                OperatorId = review.OperatorId,
                MechanicId = review.MechanicId,
                DriverId = review.DriverId,
                CarId = review.CarId,
                MechanicAcceptanceId = review.MechanicAcceptanceId,
                MechanicHandoverId = review.MechanicHandoverId,
                OperatorReviewId = review.OperatorReviewId
            };

            await _dispatcherReviewDataStore.UpdateDispatcherReview(id, reviewForUpdate);

            return RedirectToAction("BorrowIndex");
        }



        public async Task<IActionResult> PersonalIndex(int? pagenumber, string? searchString)
        {
            var reviewsResponse = await _dispatcherReviewDataStore.GetDispatcherReviews(pagenumber, searchString, null, 5, null);

            ViewBag.PageSize = reviewsResponse.PageSize;
            ViewBag.PageCount = reviewsResponse.TotalPages;
            ViewBag.TotalCount = reviewsResponse.TotalCount;
            ViewBag.CurrentPage = reviewsResponse.PageNumber;
            ViewBag.HasPreviousPage = reviewsResponse.HasPreviousPage;
            ViewBag.HasNextPage = reviewsResponse.HasNextPage;

            return View(reviewsResponse.Data);
        }

        public async Task<IActionResult> HistoryIndexForPersonalPage(int? pagenumber, string? searchString, DateTime? date)
        {
            var accountIdStr = TempData["AccountId"] as string;
            TempData.Keep("AccountId");
            int accountID = int.Parse(accountIdStr);

            var reviewsResponse = await _dispatcherReviewDataStore.GetDispatcherReviews(pagenumber, searchString, null, 7, accountID);

            ViewBag.PageSize = reviewsResponse.PageSize;
            ViewBag.PageCount = reviewsResponse.TotalPages;
            ViewBag.TotalCount = reviewsResponse.TotalCount;
            ViewBag.CurrentPage = reviewsResponse.PageNumber;
            ViewBag.HasPreviousPage = reviewsResponse.HasPreviousPage;
            ViewBag.HasNextPage = reviewsResponse.HasNextPage;

            return View(reviewsResponse.Data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var review = await _dispatcherReviewDataStore.GetDispatcherReview(id);

            return View(review);
        }

        public async Task<IActionResult> Create(double? distanceCovered, double? fuelSpended, int operatorId, int mechanicId, int driverId, int mechanicHandoverId, int mechanicAcceptanceId, int carId, int operatorReviewId)
        {
            var accountIdStr = TempData["AccountId"] as string;
            TempData.Keep("AccountId");
            var dispatcher = new DispatcherDto();
            if (int.TryParse(accountIdStr, out int accountId))
            {
                var dispatcherResponse = await _dispatcherDataStore.GetDispatchers(accountId, null);
                dispatcher = dispatcherResponse.Data.First();
            }

            var model = new DispatcherReviewForCreateDto
            {
                DistanceCovered = distanceCovered ?? 0,
                FuelSpended = fuelSpended ?? 0,
                Date = DateTime.Now.ToTashkentTime(),
                DispatcherId = dispatcher.Id,
                OperatorId = operatorId,
                MechanicId = mechanicId,
                DriverId = driverId,
                CarId = carId,
                MechanicAcceptanceId = mechanicAcceptanceId,
                MechanicHandoverId = mechanicHandoverId,
                OperatorReviewId = operatorReviewId
            };
            var car = _carDataStore.GetCarAsync(carId);
            var fuelRemaining = car.Result.RemainingFuel;
            var mediumFuelConsumption = car.Result.MeduimFuelConsumption;
            ViewBag.MediumFuelConsumption = mediumFuelConsumption;
            ViewBag.FuelRemaining = fuelRemaining;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FuelSpended,DistanceCovered,Date,DispatcherId,OperatorId,MechanicId,DriverId,ChangedFuelSpendede,ChangedDistanceCovered,MechanicHandoverId,MechanicAcceptanceId,CarId,Comment,Status,OperatorReviewId")] DispatcherReviewForCreateDto dispatcherReview)
        {
            dispatcherReview.Date = DateTime.Now.ToTashkentTime();
            if (ModelState.IsValid)
            {
                await _dispatcherReviewDataStore.CreateDispatcherReview(dispatcherReview);
                return RedirectToAction(nameof(PersonalIndex));
            }
            var care = _carDataStore.GetCarAsync(dispatcherReview.CarId);
            var fuelRemaining = care.Result.RemainingFuel;
            ViewBag.FuelRemaining = fuelRemaining;
            return View(dispatcherReview);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var review = await _dispatcherReviewDataStore.GetDispatcherReview(id);

            if (review == null)
            {
                return NotFound();
            }

            var drivers = await _driverDataStore.GetDriversAsync(1, null);
            var cars = await _carDataStore.GetCarsAsync(1, null);
            var dispatchers = await _dispatcherDataStore.GetDispatchers(null, 10);

            ViewBag.DispatcherSelectList = new SelectList(dispatchers.Data.Select(dispatcher => new
            {
                Id = dispatcher.Id,
                DisplayText = $"{dispatcher.FirstName} {dispatcher.LastName}"
            }), "Id", "DisplayText");

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


            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DispatcherReviewForUpdateDto dispatcherReview)
        {
            if (id != dispatcherReview.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldDispatcherReview = await _dispatcherReviewDataStore.GetDispatcherReview(id);
                    dispatcherReview.MechanicId = oldDispatcherReview.MechanicId;
                    dispatcherReview.MechanicAcceptanceId = oldDispatcherReview.MechanicAcceptanceId;
                    dispatcherReview.Date = oldDispatcherReview.Date;
                    dispatcherReview.OperatorReviewId = oldDispatcherReview.OperatorReviewId;
                    dispatcherReview.OperatorId = oldDispatcherReview.OperatorId;
                    dispatcherReview.MechanicHandoverId = oldDispatcherReview.MechanicHandoverId;
                    dispatcherReview.Status = StatusForDto.Completed;

                    var dr = await _dispatcherReviewDataStore.UpdateDispatcherReview(id, dispatcherReview);

                }
                catch (Exception ex)
                {
                    if (!await DispatcherReviewExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Drivers = new SelectList(await GETDrivers(), "Value", "Text");
            ViewBag.Cars = new SelectList(await GETCars(), "Value", "Text");

            return View(dispatcherReview);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var review = await _dispatcherReviewDataStore.GetDispatcherReview(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _dispatcherReviewDataStore.DeleteDispatcherReview(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Download(int year, int month)
        {
            var result = await _dispatcherReviewDataStore.GetExportFile(year, month);

            if (result == null || result.Length == 0)
            {
                return NotFound();
            }

            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Dispatcher(Xizmatlari) {month}.{year}.xlsx");
        }
        private async Task<bool> DispatcherReviewExists(int id)
        {
            var review = await _dispatcherReviewDataStore.GetDispatcherReview(id);
            return review != null;
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
    }
}
