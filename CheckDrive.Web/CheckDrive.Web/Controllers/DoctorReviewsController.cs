using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.Web.Models;
using CheckDrive.Web.Stores.DoctorReviews;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class DoctorReviewsController : Controller
    {
        private readonly IDoctorReviewDataStore _doctorReviewDataStore;

        public DoctorReviewsController(IDoctorReviewDataStore doctorReviewDataStore)
        {
            _doctorReviewDataStore = doctorReviewDataStore;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            var response = await _doctorReviewDataStore.GetDoctorReviews(pageNumber);

            ViewBag.PageSize = response.PageSize;
            ViewBag.PageCount = response.TotalPages;
            ViewBag.TotalCount = response.TotalCount;
            ViewBag.CurrentPage = response.PageNumber;
            ViewBag.HasPreviousPage = response.HasPreviousPage;
            ViewBag.HasNextPage = response.HasNextPage;

            var doctorReviews = response.Data.Select(r => new
            {
                r.Id,
                r.DriverName,
                r.DoctorName,
                IsHealthy = r.IsHealthy ? "Sog`lom" : "Kasal",
                r.Comments
            }).ToList();

            ViewBag.DoctorsReview = doctorReviews;

            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var review = await _doctorReviewDataStore.GetDoctorReview(id);
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
        public async Task<IActionResult> Create([Bind("IsHealthy,Comments,Date,DriverId,DoctorId")] DoctorReviewForCreateDto doctorReview)
        {
            if (ModelState.IsValid)
            {
                await _doctorReviewDataStore.CreateDoctorReview(doctorReview);
                return RedirectToAction(nameof(Index));
            }
            return View(doctorReview);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var review = await _doctorReviewDataStore.GetDoctorReview(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IsHealthy,Comments,Date,DriverId,DoctorId")] DoctorReviewForUpdateDto doctorReview)
        {
            if (id != doctorReview.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _doctorReviewDataStore.UpdateDoctorReview(id, doctorReview);
                }
                catch (Exception)
                {
                    if (!await DoctorReviewExists(id))
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
            return View(doctorReview);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var review = await _doctorReviewDataStore.GetDoctorReview(id);
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
            await _doctorReviewDataStore.DeleteDoctorReview(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> DoctorReviewExists(int id)
        {
            var review = await _doctorReviewDataStore.GetDoctorReview(id);
            return review != null;
        }
    }
}
