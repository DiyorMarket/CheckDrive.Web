using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.Web.Stores.DoctorReviews;
using CheckDrive.Web.Stores.Doctors;
using CheckDrive.Web.Stores.Drivers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckDrive.Web.Controllers
{
    public class PersonalDoctorReviewsController : Controller
    {
        private readonly IDoctorReviewDataStore _doctorReviewDataStore;
        private readonly IDoctorDataStore _doctorDataStore;
        private readonly IDriverDataStore _driverDataStore;

        public PersonalDoctorReviewsController(IDoctorReviewDataStore doctorReviewDataStore, IDoctorDataStore doctorDataStore, IDriverDataStore driverDataStore)
        {
            _doctorReviewDataStore = doctorReviewDataStore;
            _doctorDataStore = doctorDataStore;
            _driverDataStore = driverDataStore;
        }

        public IActionResult HelloPage()
        {
            return View();
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            var doctors = await _doctorDataStore.GetDoctors();
            var drivers = await _driverDataStore.GetDriversAsync();

   
            var response = await _doctorReviewDataStore.GetDoctorReviews( pageNumber);
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
            var doctorReview = await _doctorReviewDataStore.GetDoctorReview(id);
            if (doctorReview == null)
            {
                return NotFound();
            }
            return View(doctorReview);
        }

        public async Task<IActionResult> Create()
        {
            var doctors = await GETDoctors();
            var drivers = await GETDrivers();

            ViewBag.Drivers = new SelectList(drivers, "Value", "Text");
            ViewBag.Doctors = new SelectList(doctors, "Value", "Text");

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

        private async Task<List<SelectListItem>> GETDrivers()
        {
            var driverResponse = await _driverDataStore.GetDriversAsync();
            var drivers = driverResponse.Data
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = $"{d.FirstName} {d.LastName}"
                })
                .ToList();
            return drivers;
        }

        private async Task<List<SelectListItem>> GETDoctors()
        {
            var doctorResponse = await _doctorDataStore.GetDoctors();
            var doctors = doctorResponse.Data
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = $"{d.FirstName} {d.LastName}"
                })
                .ToList();
            return doctors;
        }
    }
}
