using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using CheckDrive.Web.Models;
using CheckDrive.Web.Stores.DoctorReviews;

namespace CheckDrive.Web.Controllers
{
    public class DoctorReviewsController : Controller
    {
        private readonly IDoctorReviewDataStore _doctorReviewDataStore;

        public DoctorReviewsController(IDoctorReviewDataStore doctorReviewDataStore)
        {
            _doctorReviewDataStore = doctorReviewDataStore;
        }

        public async Task<IActionResult> Index()
        {
            var reviews = await _doctorReviewDataStore.GetDoctorReviews();
            return View(reviews);
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
        public async Task<IActionResult> Create([Bind("IsHealthy,Comments,Date,DriverId,DoctorId")] DoctorReview doctorReview)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,IsHealthy,Comments,Date,DriverId,DoctorId")] DoctorReview doctorReview)
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
