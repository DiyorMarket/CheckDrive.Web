using CheckDrive.ApiContracts.Doctor;
using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.Web.Stores.Accounts;
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
        private readonly IAccountDataStore _accountDataStore;

        public PersonalDoctorReviewsController(IDoctorReviewDataStore doctorReviewDataStore, IDoctorDataStore doctorDataStore, IDriverDataStore driverDataStore, IAccountDataStore accountDataStore)
        {
            _doctorReviewDataStore = doctorReviewDataStore;
            _doctorDataStore = doctorDataStore;
            _driverDataStore = driverDataStore;
            _accountDataStore = accountDataStore;
        }

        public IActionResult HelloPage()
        {
            return View();
        }

        public async Task<IActionResult> Index(int? pageNumber, string? searchString)
        {
            var currentDate = DateTime.Today;
            var reviewsResponse = await _doctorReviewDataStore.GetDoctorReviewsAsync(pageNumber,null);
            var driversResponse = await _driverDataStore.GetDriversAsync(searchString, pageNumber);

            ViewBag.PageSize = driversResponse.PageSize;
            ViewBag.PageCount = driversResponse.TotalPages;
            ViewBag.TotalCount = driversResponse.TotalCount;
            ViewBag.CurrentPage = driversResponse.PageNumber;
            ViewBag.HasPreviousPage = driversResponse.HasPreviousPage;
            ViewBag.HasNextPage = driversResponse.HasNextPage;

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
            var doctorReview = await _doctorReviewDataStore.GetDoctorReviewAsync(id);
            if (doctorReview == null)
            {
                return NotFound();
            }
            return View(doctorReview);
        }

        public async Task<IActionResult> Create(int driverId, string driverName)
        {
            var accountIdStr = TempData["AccountId"] as string;
            TempData.Keep("AccountId"); // Сохранение значения AccountId между запросами

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
                    ViewBag.SelectedDriverName = driverName;
                    ViewBag.SelectedDriverId = driverId;
                    ViewBag.DoctorId = doctor.Id;  // Сохранение doctorId в ViewBag

                    return View(new DoctorReviewForCreateDto { DriverId = driverId, Date = DateTime.Now, DoctorId = doctor.Id });
                }
            }

            // Обработка случая, когда врач не найден или accountId недействителен
            return NotFound("Врач не найден для указанного аккаунта.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IsHealthy,Comments,DriverId,DoctorId")] DoctorReviewForCreateDto doctorReview)
        {
            if (ModelState.IsValid)
            {
                doctorReview.Date = DateTime.Now;
                await _doctorReviewDataStore.CreateDoctorReviewAsync(doctorReview);
                return RedirectToAction(nameof(Index));
            }

            var driver = await _driverDataStore.GetDriverAsync(doctorReview.DriverId);
            ViewBag.SelectedDriverName = $"{driver.FirstName} {driver.LastName}";
            ViewBag.SelectedDriverId = doctorReview.DriverId;

            var accountIdStr = TempData["AccountId"] as string;
            TempData.Keep("AccountId"); // Сохранение значения AccountId между запросами

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
                    ViewBag.DoctorId = doctor.Id;  // Сохранение doctorId в ViewBag при повторном отображении формы
                }
            }

            return View(doctorReview);
        }




        public async Task<IActionResult> Edit(int id)
        {
            var doctorReview = await _doctorReviewDataStore.GetDoctorReviewAsync(id);
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
            var doctorReview = await _doctorReviewDataStore.GetDoctorReviewAsync(id);
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
            await _doctorReviewDataStore.DeleteDoctorReviewAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> DoctorReviewExists(int id)
        {
            var doctorReview = await _doctorReviewDataStore.GetDoctorReviewAsync(id);
            return doctorReview != null;
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
