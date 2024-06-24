using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.MechanicHandover;
using CheckDrive.Web.Stores.Cars;
using CheckDrive.Web.Stores.DoctorReviews;
using CheckDrive.Web.Stores.Drivers;
using CheckDrive.Web.Stores.MechanicHandovers;
using CheckDrive.Web.Stores.Mechanics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckDrive.Web.Controllers
{
    public class MechanicHandoversController : Controller
    {
        private readonly IMechanicHandoverDataStore _mechanicHandoverDataStore;
        private readonly IDriverDataStore _driverDataStore;
        private readonly ICarDataStore _carDataStore;
        private readonly IMechanicDataStore _mechanicDataStore;
        private readonly IDoctorReviewDataStore _doctorReviewDataStore;

        public MechanicHandoversController(IMechanicHandoverDataStore mechanicHandoverDataStore, IDriverDataStore driverDataStore, ICarDataStore carDataStore, IMechanicDataStore mechanicDataStore, IDoctorReviewDataStore doctorReviewDataStore)
        {
            _mechanicHandoverDataStore = mechanicHandoverDataStore;
            _driverDataStore = driverDataStore;
            _carDataStore = carDataStore;
            _mechanicDataStore = mechanicDataStore;
            _doctorReviewDataStore = doctorReviewDataStore;
        }

        public async Task<IActionResult> Index(int? pageNumber, string? searchString, DateTime? date)
        {
            var response = await _mechanicHandoverDataStore.GetMechanicHandoversAsync(pageNumber, searchString, date);

            ViewBag.PageSize = response.PageSize;
            ViewBag.PageCount = response.TotalPages;
            ViewBag.TotalCount = response.TotalCount;
            ViewBag.CurrentPage = response.PageNumber;
            ViewBag.HasPreviousPage = response.HasPreviousPage;
            ViewBag.HasNextPage = response.HasNextPage;

            var mechanicHandovers = response.Data.Select(r => new
            {
                r.Id,
                IsHanded = r.IsHanded ? "Topshirildi" : "Topshirilmadi",
                r.Comments,
                Status = ((StatusForDto)r.Status) switch
                {
                    StatusForDto.Pending => "Pending",
                    StatusForDto.Completed => "Completed",
                    StatusForDto.Rejected => "Rejected",
                    StatusForDto.Unassigned => "Unassigned",
                    _ => "Unknown Status"
                },
                r.Date,
                r.Distance,
                r.DriverName,
                r.MechanicName,
                r.CarName,
                r.CarId
            }).ToList();

            ViewBag.MechanicHandovers = mechanicHandovers;

            return View();
        }

        public async Task<IActionResult> PersonalIndex(string? searchString, int? pageNumber)
        {
            var response = await _mechanicHandoverDataStore.GetMechanicHandoversAsync(null, null, null);
            var doctorReviewsResponse = await _doctorReviewDataStore.GetDoctorReviewsAsync(null, searchString, null);

            var filteredDoctorReviews = doctorReviewsResponse.Data
                .Where(dr => dr.Date.Date == DateTime.Today)
                .Where(dr => dr.IsHealthy == true)
                .ToList();

            int pageSize = 10;
            pageNumber = pageNumber ?? 1;

            int totalCount = filteredDoctorReviews.Count;
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            bool hasPreviousPage = pageNumber > 1;
            bool hasNextPage = pageNumber < totalPages;

            var paginatedDoctorReviews = filteredDoctorReviews
                .Skip((pageNumber.Value - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var mechanicHandovers = new List<MechanicHandoverDto>();

            foreach (var doctor in paginatedDoctorReviews)
            {
                var review = response.Data.FirstOrDefault(r => r.DriverId == doctor.DriverId);

                if (review != null)
                {
                    if (review.Date.HasValue && review.Date.Value.Date == DateTime.Today)
                    {
                        mechanicHandovers.Add(new MechanicHandoverDto
                        {
                            DriverId = review.DriverId,
                            DriverName = doctor.DriverName,
                            MechanicName = review.MechanicName,
                            IsHanded = review.IsHanded,
                            Distance = review.Distance,
                            Comments = review.Comments,
                            Date = review.Date
                        });
                    }
                    else
                    {
                        mechanicHandovers.Add(new MechanicHandoverDto
                        {
                            DriverId = doctor.DriverId,
                            DriverName = doctor.DriverName,
                            MechanicName = "",
                            IsHanded = false,
                            Distance = review.Distance,
                            Comments = "",
                            Date = null
                        });
                    }
                }
                else
                {
                    mechanicHandovers.Add(new MechanicHandoverDto
                    {
                        DriverId = doctor.DriverId,
                        DriverName = doctor.DriverName,
                        MechanicName = "",
                        IsHanded = false,
                        Distance = 0,
                        Comments = "",
                        Date = null
                    });
                }
            }

            ViewBag.PageSize = pageSize;
            ViewBag.PageCount = totalPages;
            ViewBag.TotalCount = totalCount;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.HasPreviousPage = hasPreviousPage;
            ViewBag.HasNextPage = hasNextPage;
            ViewBag.SearchString = searchString;

            return View(mechanicHandovers);
        }



        public async Task<IActionResult> Create(int? driverId)
        {
            var mechanics = await GETMechanics();
            var drivers = await GETDrivers();
            var cars = await GETCars();

            var doctorReviews = await _doctorReviewDataStore.GetDoctorReviewsAsync(null, null, null);
            var mechanicHandovers = await _mechanicHandoverDataStore.GetMechanicHandoversAsync();

            var accountIdStr = TempData["AccountId"] as string;
            TempData.Keep("AccountId");

            if (int.TryParse(accountIdStr, out int accountId))
            {
                var mechanicResponse = await _mechanicDataStore.GetMechanics(accountId);
                var mechanic = mechanicResponse.Data.First();
                if (mechanic != null)
                {
                    var healthyDrivers = doctorReviews.Data
                        .Where(dr => dr.IsHealthy.HasValue && dr.IsHealthy.Value && dr.Date.Date == DateTime.Today)
                        .Select(dr => dr.DriverId)
                        .ToList();

                    var handedDrivers = mechanicHandovers.Data
                        .Where(ma => ma.Date.HasValue && ma.Date.Value.Date == DateTime.Today)
                        .Select(ma => ma.DriverId)
                        .ToList();

                    var filteredDrivers = drivers
                        .Where(d => healthyDrivers.Contains(int.Parse(d.Value)) && !handedDrivers.Contains(int.Parse(d.Value)))
                        .ToList();

                    var usedCarIds = mechanicHandovers.Data
                        .Where(mh => mh.Date.HasValue && mh.Date.Value.Date == DateTime.Today && mh.IsHanded == true)
                        .Select(mh => mh.CarId)
                        .ToList();

                    var filteredCars = cars
                        .Where(c => !usedCarIds.Contains(int.Parse(c.Value)))
                        .ToList();

                    ViewBag.Mechanics = new SelectList(mechanics, "Value", "Text");
                    ViewBag.Drivers = new SelectList(filteredDrivers, "Value", "Text", driverId);
                    ViewBag.Cars = new SelectList(filteredCars, "Value", "Text");

                    var selectedDriverName = filteredDrivers.FirstOrDefault(d => d.Value == driverId.ToString())?.Text;
                    ViewBag.SelectedDriverName = selectedDriverName ?? string.Empty;
                    ViewBag.SelectedDriverId = driverId;

                    return View(new MechanicHandoverForCreateDto { DriverId = driverId ?? 0, MechanicId = mechanic.Id });
                }
            }

            return NotFound("Механик не найден для указанного аккаунта.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IsHanded,Comments,MechanicId,Distance,CarId,DriverId")] MechanicHandoverForCreateDto mechanicHandoverForCreateDto)
        {
            if (ModelState.IsValid)
            {
                if (mechanicHandoverForCreateDto.IsHanded == false)
                {
                    mechanicHandoverForCreateDto.Status = StatusForDto.Rejected;
                }

                mechanicHandoverForCreateDto.Date = DateTime.Now;
                await _mechanicHandoverDataStore.CreateMechanicHandoverAsync(mechanicHandoverForCreateDto);
                return RedirectToAction(nameof(PersonalIndex));
            }

            var mechanics = await GETMechanics();
            var drivers = await GETDrivers();
            var cars = await GETCars();
            ViewBag.Mechanics = new SelectList(mechanics, "Value", "Text");
            ViewBag.Drivers = new SelectList(drivers, "Value", "Text", mechanicHandoverForCreateDto.DriverId);
            ViewBag.Cars = new SelectList(cars, "Value", "Text");

            var selectedDriverName = drivers.FirstOrDefault(d => d.Value == mechanicHandoverForCreateDto.DriverId.ToString())?.Text;
            ViewBag.SelectedDriverName = selectedDriverName ?? string.Empty;
            ViewBag.SelectedDriverId = mechanicHandoverForCreateDto.DriverId;

            return View(mechanicHandoverForCreateDto);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MechanicHandoverForUpdateDto mechanicHandover)
        {
            if (id != mechanicHandover.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _mechanicHandoverDataStore.UpdateMechanicHandoverAsync(id, mechanicHandover);
                }
                catch (Exception)
                {
                    if (!await MechanicHandoverExists(id))
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

            ViewBag.Drivers = new SelectList(await GETDrivers(), "Value", "Text");
            ViewBag.Cars = new SelectList(await GETCars(), "Value", "Text");

            return View(mechanicHandover);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var mechanicHandover = await _mechanicHandoverDataStore.GetMechanicHandoverAsync(id);
            if (mechanicHandover == null)
            {
                return NotFound();
            }
            return View(mechanicHandover);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mechanicHandoverDataStore.DeleteMechanicHandoverAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> MechanicHandoverExists(int id)
        {
            var mechanicAcceptance = await _mechanicHandoverDataStore.GetMechanicHandoverAsync(id);
            return mechanicAcceptance != null;
        }

        private async Task<List<SelectListItem>> GETMechanics()
        {
            var mechanicResponse = await _mechanicDataStore.GetMechanicsAsync();
            var mechanics = mechanicResponse.Data
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = $"{d.FirstName} {d.LastName}"
                })
                .ToList();
            return mechanics;
        }
        private async Task<List<SelectListItem>> GETCars()
        {
            var carResponse = await _carDataStore.GetCarsAsync(null, null);
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