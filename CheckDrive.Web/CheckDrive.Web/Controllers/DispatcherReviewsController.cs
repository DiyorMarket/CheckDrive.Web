 using CheckDrive.Web.Models;
using CheckDrive.Web.Stores.DispatcherReviews;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class DispatcherReviewsController : Controller
    {
        private readonly IDispatcherReviewDataStore _dispatcherReviewDataStore;

        public DispatcherReviewsController(IDispatcherReviewDataStore dispatcherReviewDataStore)
        {
            _dispatcherReviewDataStore = dispatcherReviewDataStore;
        }

        public async Task<IActionResult> Index(int? pagenumber)
        {
            var response = await _dispatcherReviewDataStore.GetDispatcherReviews(pagenumber);
            

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

        public async Task<IActionResult> Details(int id)
        {
            var review = await _dispatcherReviewDataStore.GetDispatcherReview(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        public IActionResult Create()
        {
            return View();
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
