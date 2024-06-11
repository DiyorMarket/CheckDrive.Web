using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.Web.Models;
using CheckDrive.Web.Stores.DoctorReviews;
using CheckDrive.Web.Stores.Doctors;
using CheckDrive.Web.Stores.Drivers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IActionResult> Index(int? pageNumber,string? searchString)
        {
            var currentDate = DateTime.Today;
            var reviewsResponse = await _doctorReviewDataStore.GetDoctorReviews(pageNumber);
            var driversResponse = await _driverDataStore.GetDriversAsync(searchString);

            var doctorReviews = new List<DoctorReviewDto>();

            if (reviewsResponse.Data.Any())
            {
                doctorReviews = driversResponse.Data.Select(driver =>
                {
                    var review = reviewsResponse.Data.FirstOrDefault(r => r.DriverId == driver.Id && r.Date.Date == currentDate);
                    if (review != null)
                    {
                        return new DoctorReviewDto
                        {
                            DriverId = driver.Id,
                            DriverName = $"{driver.FirstName} {driver.LastName}",
                            DoctorName = review.DoctorName,
                            IsHealthy = review.IsHealthy,
                            Comments = review.Comments,
                            Date = review.Date
                        };
                    }
                    return new DoctorReviewDto
                    {
                        DriverId = driver.Id,
                        DriverName = $"{driver.FirstName} {driver.LastName}",
                        DoctorName = "",
                        IsHealthy = null,
                        Comments = "",
                        Date = currentDate
                    };
                }).ToList();
            }
            else
            {
                doctorReviews = driversResponse.Data.Select(driver => new DoctorReviewDto
                {
                    DriverId = driver.Id,
                    DriverName = $"{driver.FirstName} {driver.LastName}",
                    DoctorName = "",
                    IsHealthy = null,
                    Comments = "",
                    Date = currentDate
                }).ToList();
            }

            return View(doctorReviews);
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

        public async Task<IActionResult> Create(int driverId, string driverName)
        {
            var doctors = await GETDoctors();

            ViewBag.Doctors = new SelectList(doctors, "Value", "Text");
            ViewBag.SelectedDriverName = driverName;
            ViewBag.SelectedDriverId = driverId;

            return View(new DoctorReviewForCreateDto { DriverId = driverId, Date = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IsHealthy,Comments,DriverId,DoctorId")] DoctorReviewForCreateDto doctorReview)
        {
            if (ModelState.IsValid)
            {
                // Если значение IsHealthy не установлено, установите его в false
                if (doctorReview.IsHealthy == null)
                {
                    doctorReview.IsHealthy = false;
                }

                doctorReview.Date = DateTime.Now;
                await _doctorReviewDataStore.CreateDoctorReview(doctorReview);
                return RedirectToAction(nameof(Index));
            }

            var doctors = await GETDoctors();
            ViewBag.Doctors = new SelectList(doctors, "Value", "Text");

            var driver = await _driverDataStore.GetDriverAsync(doctorReview.DriverId);
            ViewBag.SelectedDriverName = $"{driver.FirstName} {driver.LastName}";
            ViewBag.SelectedDriverId = doctorReview.DriverId;

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
