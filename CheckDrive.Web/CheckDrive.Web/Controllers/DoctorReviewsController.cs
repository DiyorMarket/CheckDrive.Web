using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.Web.Stores.Accounts;
using CheckDrive.Web.Stores.DoctorReviews;
using CheckDrive.Web.Stores.Doctors;
using CheckDrive.Web.Stores.Drivers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckDrive.Web.Controllers
{
    public class DoctorReviewsController : Controller
    {
        private readonly IDoctorReviewDataStore _doctorReviewDataStore;
        private readonly IDoctorDataStore _doctorDataStore;
        private readonly IDriverDataStore _driverDataStore;
        private readonly IAccountDataStore _accountDataStore;

        public DoctorReviewsController(IDoctorReviewDataStore doctorReviewDataStore, IDoctorDataStore doctorDataStore, IDriverDataStore driverDataStore, IAccountDataStore accountDataStore)
        {
            _doctorReviewDataStore = doctorReviewDataStore;
            _doctorDataStore = doctorDataStore;
            _driverDataStore = driverDataStore;
            _accountDataStore = accountDataStore;
        }

        public async Task<IActionResult> Index(int? pageNumber, string? searchString, DateTime? date)
        {
            var response = await _doctorReviewDataStore.GetDoctorReviewsAsync(pageNumber, searchString, date, null, 1);

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
                r.Date,
                IsHealthy = (bool)r.IsHealthy ? "Sog`lom" : "Kasal",
                r.Comments
            }).ToList();

            ViewBag.DoctorsReview = doctorReviews;

            return View();
        }

        public async Task<IActionResult> PersonalIndex(int? pageNumber, string? searchString)
        {
            var reviewsResponse = await _doctorReviewDataStore.GetDoctorReviewsAsync(pageNumber, searchString, null, null, 3);

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
            var review = await _doctorReviewDataStore.GetDoctorReviewAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        public async Task<IActionResult> Create(int driverId, string driverName)
        {
            var accountIdStr = TempData["AccountId"] as string;
            TempData.Keep("AccountId");

            if (int.TryParse(accountIdStr, out int accountId))
            {
                var doctorResponse = await _doctorDataStore.GetDoctors(accountId);
                var doctor = doctorResponse.Data.FirstOrDefault();
                if (doctor != null)
                {
                    var doctors = new List<SelectListItem>
            {
                new SelectListItem { Value = doctor.Id.ToString(), Text = $"{doctor.FirstName} {doctor.LastName}" }
            };

                    var driversNotUsedToday = await GetDriversNotUsedToday();

                    ViewBag.Doctors = new SelectList(doctors, "Value", "Text");
                    ViewBag.SelectedDriverName = driverName;
                    ViewBag.SelectedDriverId = driverId;
                    ViewBag.DoctorId = doctor.Id;
                    ViewBag.Drivers = new SelectList(driversNotUsedToday, "Value", "Text");

                    return View(new DoctorReviewForCreateDto { DriverId = driverId, Date = DateTime.Now, DoctorId = doctor.Id });
                }
            }

            return NotFound("Bu akkauntdagi shifokor topilmadi");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IsHealthy,Comments,DriverId,DoctorId")] DoctorReviewForCreateDto doctorReview)
        {
            if (ModelState.IsValid)
            {
                doctorReview.Date = DateTime.Now;
                await _doctorReviewDataStore.CreateDoctorReviewAsync(doctorReview);
                return RedirectToAction(nameof(PersonalIndex));
            }

            var driver = await _driverDataStore.GetDriverAsync(doctorReview.DriverId);
            ViewBag.SelectedDriverName = $"{driver.FirstName} {driver.LastName}";
            ViewBag.SelectedDriverId = doctorReview.DriverId;

            var accountIdStr = TempData["AccountId"] as string;
            TempData.Keep("AccountId");

            if (int.TryParse(accountIdStr, out int accountId))
            {
                var doctorResponse = await _doctorDataStore.GetDoctors(accountId);
                var doctor = doctorResponse.Data.FirstOrDefault();
                if (doctor != null)
                {
                    var doctors = new List<SelectListItem>
            {
                new SelectListItem { Value = doctor.Id.ToString(), Text = $"{doctor.FirstName} {doctor.LastName}" }
            };

                    ViewBag.Doctors = new SelectList(doctors, "Value", "Text");
                    ViewBag.DoctorId = doctor.Id;
                }
            }

            return View(doctorReview);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var review = await _doctorReviewDataStore.GetDoctorReviewAsync(id);
            var doctorReviews = await _doctorReviewDataStore.GetDoctorReviewsAsync(null, null, review.Date.Date, null, null);
            var doctorReviewsForDoctor = await _doctorReviewDataStore.GetDoctorReviewsAsync(null, null, review.Date.Date, null, 3);

            var driverIds = doctorReviews.Data.Select(ma => ma.DriverId).ToHashSet();
            var filteredDoctorResponse = doctorReviewsForDoctor.Data.Where(or => !driverIds.Contains(or.DriverId)).ToList();
            filteredDoctorResponse.Add(review);

            if (review == null)
            {
                return NotFound();
            }

            ViewBag.DriverSelectList = new SelectList(filteredDoctorResponse, "DriverId", "DriverName", review.DriverId);

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
                    await _doctorReviewDataStore.UpdateDoctorReviewAsync(id, doctorReview);
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
            var review = await _doctorReviewDataStore.GetDoctorReviewAsync(id);
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
            await _doctorReviewDataStore.DeleteDoctorReviewAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> DoctorReviewExists(int id)
        {
            var review = await _doctorReviewDataStore.GetDoctorReviewAsync(id);
            return review != null;
        }

        private async Task<List<SelectListItem>> GetDriversNotUsedToday()
        {
            var doctorReviews = await _doctorReviewDataStore.GetDoctorReviewsAsync(null, null, null, null, 1);
            var today = DateTime.Today;
            var usedDriverIds = doctorReviews.Data
                .Where(dr => dr.Date.Date == today)
                .Select(dr => dr.DriverId)
                .ToList();

            var drivers = await GETDrivers();
            var driversNotUsedToday = drivers
                .Where(d => !usedDriverIds.Contains(int.Parse(d.Value)))
                .ToList();

            return driversNotUsedToday;
        }
        private async Task<List<SelectListItem>> GETDrivers()
        {
            var driverResponse = await _driverDataStore.GetDriversAsync(null, null);
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
