using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.Web.Stores.DoctorReviews;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class PersonalDoctorReviewsController(IDoctorReviewDataStore doctorReviewDataStore) : Controller
    {
        private readonly IDoctorReviewDataStore _doctorReviewDataStore = doctorReviewDataStore;
        public async Task<IActionResult> Index()
        {
            var doctorReviews = await _doctorReviewDataStore.GetDoctorReviews();

            if (doctorReviews is null)
            {
                return BadRequest();
            }

            ViewBag.DoctorsReview = doctorReviews.Data;
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var doctorReview = await _doctorReviewDataStore.GetDoctorReview(id);
            if (doctorReview == null)
            {
                return NotFound();
            }
            return View(doctorReview);
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
            var doctorReview = await _doctorReviewDataStore.GetDoctorReview(id);
            if (doctorReview == null)
            {
                return NotFound();
            }
            return View(doctorReview);
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
            var doctorReview = await _doctorReviewDataStore.GetDoctorReview(id);
            if (doctorReview == null)
            {
                return NotFound();
            }
            return View(doctorReview);
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
            var doctorReview = await _doctorReviewDataStore.GetDoctorReview(id);
            return doctorReview != null;
        }
    }
}
