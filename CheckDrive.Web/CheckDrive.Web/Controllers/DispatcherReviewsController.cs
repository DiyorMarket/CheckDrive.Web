using CheckDrive.ApiContracts.Dispatcher;
using CheckDrive.ApiContracts.DispatcherReview;
using CheckDrive.Web.Models;
using CheckDrive.Web.Stores.Cars;
using CheckDrive.Web.Stores.DispatcherReviews;
using CheckDrive.Web.Stores.Dispatchers;
using CheckDrive.Web.Stores.MechanicAcceptances;
using CheckDrive.Web.Stores.MechanicHandovers;
using CheckDrive.Web.Stores.OperatorReviews;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class DispatcherReviewsController : Controller
    {
        private readonly IDispatcherReviewDataStore _dispatcherReviewDataStore;
        private readonly IMechanicAcceptanceDataStore _mechanicAcceptanceDataStore;
        private readonly IMechanicHandoverDataStore _mechanicHandoverDataStore;
        private readonly IOperatorReviewDataStore _operatorDataStore;
        private readonly IDispatcherDataStore _dispatcherDataStore;
        private readonly ICarDataStore _carDataStore;

        public DispatcherReviewsController(
            IDispatcherReviewDataStore dispatcherReviewDataStore,
            IMechanicAcceptanceDataStore mechanicAcceptanceDataStore,
            IOperatorReviewDataStore operatorDataStore,
            IMechanicHandoverDataStore mechanicHandoverDataStore,
            IDispatcherDataStore dispatcherDataStore,
            ICarDataStore carDataStore)
        {
            _dispatcherReviewDataStore = dispatcherReviewDataStore;
            _mechanicAcceptanceDataStore = mechanicAcceptanceDataStore;
            _operatorDataStore = operatorDataStore;
            _mechanicHandoverDataStore = mechanicHandoverDataStore;
            _carDataStore = carDataStore;
            _dispatcherDataStore = dispatcherDataStore;
        }

        public async Task<IActionResult> Index(int? pagenumber, string? searchString, DateTime? date)
        {
            var response = await _dispatcherReviewDataStore.GetDispatcherReviews(pagenumber, searchString, date);


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

        public async Task<IActionResult> PersonalIndex(int? pagenumber)
        {
            var reviewsResponse = await _dispatcherReviewDataStore.GetDispatcherReviews(pagenumber, null, null);
            var mechanicAcceptanceResponse = await _mechanicAcceptanceDataStore.GetMechanicAcceptancesAsync();
            var mechanicHandoverResponse = await _mechanicHandoverDataStore.GetMechanicHandoversAsync();
            var operatorResoponse = await _operatorDataStore.GetOperatorReviews(null, null, null);
            var carResponse = await _carDataStore.GetCarsAsync(null, null);

            var mechanicAcceptances = mechanicAcceptanceResponse.Data
                .Where(m => m.Date.Value.Date == DateTime.Today)
                .Where(m => m.IsAccepted == true)
                .ToList();

            var dispatchers = new List<DispatcherReviewDto>();

            foreach (var mechanicAcceptance in mechanicAcceptances)
            {
                var mechanicHandoverReview = mechanicHandoverResponse.Data.FirstOrDefault(m => m.DriverId == mechanicAcceptance.DriverId && m.Date.Value.Date == DateTime.Today);
                var operatorReview = operatorResoponse.Data.FirstOrDefault(m => m.DriverId == mechanicAcceptance.DriverId && m.Date.Value.Date == DateTime.Today);
                var carReview = carResponse.Data.FirstOrDefault(c => c.Id == mechanicAcceptance.CarId);
                var review = reviewsResponse.Data.FirstOrDefault(r => r.DriverId == mechanicAcceptance.DriverId);
                if (review != null)
                {
                    if (review.Date == DateTime.Today)
                    {
                        dispatchers.Add(new DispatcherReviewDto
                        {
                            DriverId = review.DriverId,
                            DriverName = mechanicAcceptance.DriverName,
                            CarId = review.CarId,
                            CarName = review.CarName,
                            CarMeduimFuelConsumption = review.CarMeduimFuelConsumption,
                            FuelSpended = review.FuelSpended,
                            DistanceCovered = review.DistanceCovered,
                            InitialDistance = review.InitialDistance,
                            FinalDistance = review.FinalDistance,
                            PouredFuel = review.PouredFuel,
                            OperatorName = review.OperatorName,
                            DispatcherName = review.DispatcherName,
                            MechanicName = review.MechanicName,
                            Date = review.Date,
                            DispatcherId = review.DispatcherId,
                            MechanicAcceptanceId = review.MechanicAcceptanceId,
                            MechanicHandoverId = review.MechanicHandoverId,
                            OperatorId = review.OperatorId,
                            MechanicId = review.MechanicId,
                        });
                    }
                    else
                    {
                        dispatchers.Add(new DispatcherReviewDto
                        {
                            DriverId = mechanicAcceptance.DriverId,
                            DriverName = mechanicAcceptance.DriverName,
                            CarId = mechanicAcceptance.CarId,
                            CarName = mechanicAcceptance.CarName,
                            CarMeduimFuelConsumption = carReview.MeduimFuelConsumption,
                            FuelSpended = (mechanicAcceptance.Distance - mechanicHandoverReview.Distance) / carReview.MeduimFuelConsumption,
                            DistanceCovered = mechanicAcceptance.Distance - mechanicHandoverReview.Distance,
                            InitialDistance = mechanicHandoverReview.Distance,
                            FinalDistance = mechanicAcceptance.Distance,
                            PouredFuel = operatorReview.OilAmount ?? 0,
                            OperatorName = operatorReview.OperatorName,
                            DispatcherName = "",
                            MechanicName = mechanicAcceptance.MechanicName,
                            Date = DateTime.Today,
                            DispatcherId = review.DispatcherId,
                            MechanicAcceptanceId = mechanicAcceptance.Id,
                            MechanicHandoverId = mechanicHandoverReview.Id,
                            OperatorId = operatorReview.OperatorId,
                            MechanicId = mechanicAcceptance.MechanicId
                        });
                    }
                }
                else
                {
                    dispatchers.Add(new DispatcherReviewDto
                    {
                        DriverId = mechanicAcceptance.DriverId,
                        DriverName = mechanicAcceptance.DriverName,
                        CarId = mechanicAcceptance.CarId,
                        CarName = mechanicAcceptance.CarName,
                        CarMeduimFuelConsumption = carReview.MeduimFuelConsumption,
                        FuelSpended = (mechanicAcceptance.Distance - mechanicHandoverReview.Distance) / carReview.MeduimFuelConsumption,
                        DistanceCovered = mechanicAcceptance.Distance - mechanicHandoverReview.Distance,
                        InitialDistance = mechanicHandoverReview.Distance,
                        FinalDistance = mechanicAcceptance.Distance,
                        PouredFuel = operatorReview.OilAmount ?? 0,
                        OperatorName = operatorReview.OperatorName,
                        DispatcherName = "",
                        MechanicName = mechanicAcceptance.MechanicName,
                        Date = DateTime.Today,
                        DispatcherId = review.DispatcherId,
                        MechanicAcceptanceId = mechanicAcceptance.Id,
                        MechanicHandoverId = mechanicHandoverReview.Id,
                        OperatorId = operatorReview.OperatorId,
                        MechanicId = mechanicAcceptance.MechanicId
                    });
                }
            }

            return View(dispatchers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var review = await _dispatcherReviewDataStore.GetDispatcherReview(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        public async Task<IActionResult> Create(double? distanceCovered, double? fuelSpended, int operatorId, int mechanicId, int driverId)
        {
            var accountIdStr = TempData["AccountId"] as string;
            TempData.Keep("AccountId");
            var dispatcher = new DispatcherDto();
            if (int.TryParse(accountIdStr, out int accountId))
            {
                var dispatcherResponse = await _dispatcherDataStore.GetDispatchers(accountId);
                dispatcher = dispatcherResponse.Data.First();
            }
            var model = new DispatcherReview
            {
                DistanceCovered = distanceCovered ?? 0,
                FuelSpended = fuelSpended ?? 0,
                Date = DateTime.Today,
                DispatcherId = dispatcher.Id,
                OperatorId = operatorId,
                MechanicId = mechanicId,
                DriverId = driverId
            };

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FuelSpended,DistanceCovered,Date,DispatcherId,OperatorId,MechanicId,DriverId")] DispatcherReview dispatcherReview)
        {
            if (ModelState.IsValid)
            {
                await _dispatcherReviewDataStore.CreateDispatcherReview(dispatcherReview);
                return RedirectToAction(nameof(Index));
            }
            return View(dispatcherReview);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var review = await _dispatcherReviewDataStore.GetDispatcherReview(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FuelSpended,DistanceCovered,Date,DispatcherId,OperatorId,MechanicId,DriverId")] DispatcherReview dispatcherReview)
        {
            if (id != dispatcherReview.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _dispatcherReviewDataStore.UpdateDispatcherReview(id, dispatcherReview);
                }
                catch (Exception)
                {
                    if (!await DispatcherReviewExists(id))
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

        private async Task<bool> DispatcherReviewExists(int id)
        {
            var review = await _dispatcherReviewDataStore.GetDispatcherReview(id);
            return review != null;
        }
    }
}
