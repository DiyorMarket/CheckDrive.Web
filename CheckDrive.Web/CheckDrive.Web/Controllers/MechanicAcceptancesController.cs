using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.Web.Stores.Cars;
using CheckDrive.Web.Stores.Drivers;
using CheckDrive.Web.Stores.MechanicAcceptances;
using CheckDrive.Web.Stores.MechanicHandovers;
using CheckDrive.Web.Stores.Mechanics;
using CheckDrive.Web.Stores.OperatorReviews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckDrive.Web.Controllers
{
    public class MechanicAcceptancesController : Controller
    {
        private readonly IMechanicAcceptanceDataStore _mechanicAcceptanceDataStore;
        private readonly IDriverDataStore _driverDataStore;
        private readonly ICarDataStore _carDataStore;
        private readonly IMechanicDataStore _mechanicDataStore;
        private readonly IOperatorReviewDataStore _operatorReviewDataStore;
        private readonly IMechanicHandoverDataStore _mechanicHandoverDataStore;

        public MechanicAcceptancesController(IMechanicAcceptanceDataStore mechanicAcceptanceDataStore, IDriverDataStore driverDataStore, ICarDataStore carDataStore, IMechanicDataStore mechanicDataStore, IOperatorReviewDataStore operatorReviewDataStore, IMechanicHandoverDataStore mechanicHandoverDataStore)
        {
            _mechanicAcceptanceDataStore = mechanicAcceptanceDataStore;
            _driverDataStore = driverDataStore;
            _carDataStore = carDataStore;
            _mechanicDataStore = mechanicDataStore;
            _operatorReviewDataStore = operatorReviewDataStore;
            _mechanicHandoverDataStore = mechanicHandoverDataStore;
        }

        public async Task<IActionResult> Index(int? pageNumber, string? searchString, DateTime? date)
        {

            var response = await _mechanicAcceptanceDataStore.GetMechanicAcceptancesAsync(pageNumber, searchString, date);

            ViewBag.PageSize = response.PageSize;
            ViewBag.PageCount = response.TotalPages;
            ViewBag.TotalCount = response.TotalCount;
            ViewBag.CurrentPage = response.PageNumber;
            ViewBag.HasPreviousPage = response.HasPreviousPage;
            ViewBag.HasNextPage = response.HasNextPage;

            var mechanicAcceptances = response.Data.Select(r => new
            {
                r.Id,
                IsAccepted = (bool)r.IsAccepted ? "Qabul qilindi" : "Rad etildi",
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

            ViewBag.MechanicAcceptances = mechanicAcceptances;

            return View();
        }


        public async Task<IActionResult> PersonalIndex(string? searchString, int? pageNumber)
        {
            var response = await _mechanicAcceptanceDataStore.GetMechanicAcceptancesAsync(null, null, null);
            var operatorReviewsResponse = await _operatorReviewDataStore.GetOperatorReviews(null, searchString, null);
            var carHandoversResponse = await _mechanicHandoverDataStore.GetMechanicHandoversAsync();
            var carsResponse = await _carDataStore.GetCarsAsync(null, null);

            var carsDict = carsResponse.Data.ToDictionary(c => c.Id, c => $"{c.Model} ({c.Number})");

            var filteredOperatorReviews = operatorReviewsResponse.Data
                .Where(dr => dr.Date.Value.Date == DateTime.Today)
                .Where(dr => dr.IsGiven == true)
                .ToList();

            int pageSize = 10;
            pageNumber = pageNumber ?? 1;

            int totalCount = filteredOperatorReviews.Count;
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            bool hasPreviousPage = pageNumber > 1;
            bool hasNextPage = pageNumber < totalPages;

            var paginatedOperatorReviews = filteredOperatorReviews
                .Skip((pageNumber.Value - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var mechanicAcceptance = new List<MechanicAcceptanceDto>();

            foreach (var operatorr in paginatedOperatorReviews)
            {
                var review = response.Data.FirstOrDefault(r => r.DriverId == operatorr.DriverId);
                var carHandover = carHandoversResponse.Data.FirstOrDefault(ch => ch.DriverId == operatorr.DriverId && ch.Date.Date == DateTime.Today);

                int carId = carHandover?.CarId ?? 0;
                string carName = carId != 0 && carsDict.ContainsKey(carId) ? carsDict[carId] : string.Empty;

                if (review != null)
                {
                    if (review.Date.HasValue && review.Date.Value.Date == DateTime.Today)
                    {
                        mechanicAcceptance.Add(new MechanicAcceptanceDto
                        {
                            DriverId = review.DriverId,
                            DriverName = operatorr.DriverName,
                            MechanicName = review.MechanicName,
                            IsAccepted = review.IsAccepted,
                            Distance = review.Distance,
                            Comments = review.Comments,
                            Date = review.Date,
                            CarId = carId,
                            CarName = carName
                        });
                    }
                    else
                    {
                        mechanicAcceptance.Add(new MechanicAcceptanceDto
                        {
                            DriverId = operatorr.DriverId,
                            DriverName = operatorr.DriverName,
                            MechanicName = "",
                            IsAccepted = false,
                            Distance = 0,
                            Comments = "",
                            Date = null,
                            CarId = carId,
                            CarName = carName
                        });
                    }
                }
                else
                {
                    mechanicAcceptance.Add(new MechanicAcceptanceDto
                    {
                        DriverId = operatorr.DriverId,
                        DriverName = operatorr.DriverName,
                        MechanicName = "",
                        IsAccepted = false,
                        Distance = 0,
                        Comments = "",
                        Date = null,
                        CarId = carId,
                        CarName = carName
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

            return View(mechanicAcceptance);
        }
        public async Task<IActionResult> Create(int? driverId, int? carId)
        {
            var mechanics = await GETMechanics();
            var drivers = await GETDrivers();
            var cars = await GETCars();

            var operatorReviews = await _operatorReviewDataStore.GetOperatorReviews(null, null, null);
            var mechanicAcceptances = await _mechanicAcceptanceDataStore.GetMechanicAcceptancesAsync();

            var accountIdStr = TempData["AccountId"] as string;
            TempData.Keep("AccountId");

            if (int.TryParse(accountIdStr, out int accountId))
            {
                var mechanicResponse = await _mechanicDataStore.GetMechanics(accountId);
                var mechanic = mechanicResponse.Data.FirstOrDefault();

                if (mechanic != null)
                {
                    var healthyDrivers = operatorReviews.Data
                        .Where(dr => dr.IsGiven.HasValue && dr.IsGiven.Value && dr.Date.HasValue && dr.Date.Value.Date == DateTime.Today)
                        .Select(dr => dr.DriverId)
                        .ToList();

                    var acceptedDrivers = mechanicAcceptances.Data
                        .Where(ma => ma.Date.HasValue && ma.Date.Value.Date == DateTime.Today)
                        .Select(ma => ma.DriverId)
                        .ToList();

                    var filteredDrivers = drivers
                        .Where(d => healthyDrivers.Contains(int.Parse(d.Value)) && !acceptedDrivers.Contains(int.Parse(d.Value)))
                        .ToList();

                    if (driverId == null && filteredDrivers.Any())
                    {
                        driverId = int.Parse(filteredDrivers.First().Value);
                    }

                    filteredDrivers = filteredDrivers.Where(d => d.Value != driverId.ToString()).ToList();

                    if (carId == null && cars.Any())
                    {
                        carId = int.Parse(cars.First().Value);
                    }

                    ViewBag.Mechanics = new SelectList(mechanics, "Value", "Text");
                    ViewBag.Drivers = new SelectList(filteredDrivers, "Value", "Text", driverId);
                    ViewBag.Cars = new SelectList(cars, "Value", "Text", carId);

                    var selectedDriverName = drivers.FirstOrDefault(d => d.Value == driverId.ToString())?.Text;
                    ViewBag.SelectedDriverName = selectedDriverName ?? string.Empty;
                    ViewBag.SelectedDriverId = driverId;

                    var selectedCar = cars.FirstOrDefault(c => c.Value == carId.ToString())?.Text;
                    ViewBag.SelectedCar = selectedCar ?? string.Empty;
                    ViewBag.SelectedCarId = carId;

                    var model = new MechanicAcceptanceForCreateDto
                    {
                        DriverId = driverId ?? 0,
                        MechanicId = mechanic.Id,
                        CarId = carId ?? 0
                    };

                    return View(model);
                }
            }

            return NotFound("Mechanic not found for the specified account.");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IsAccepted,Comments,MechanicId,Distance,CarId,DriverId")] MechanicAcceptanceForCreateDto mechanicAcceptanceForCreateDto)
        {
            if (ModelState.IsValid)
            {
                if (mechanicAcceptanceForCreateDto.IsAccepted == null)
                {
                    mechanicAcceptanceForCreateDto.IsAccepted = false;
                }

                mechanicAcceptanceForCreateDto.Date = DateTime.Now;

                await _mechanicAcceptanceDataStore.CreateMechanicAcceptanceAsync(mechanicAcceptanceForCreateDto);
                return RedirectToAction(nameof(PersonalIndex));
            }

            var mechanics = await GETMechanics();
            var drivers = await GETDrivers();
            var cars = await GETCars();

            ViewBag.Mechanics = new SelectList(mechanics, "Value", "Text");
            ViewBag.Drivers = new SelectList(drivers, "Value", "Text", mechanicAcceptanceForCreateDto.DriverId);
            ViewBag.Cars = new SelectList(cars, "Value", "Text", mechanicAcceptanceForCreateDto.CarId);

            var selectedDriverName = drivers.FirstOrDefault(d => d.Value == mechanicAcceptanceForCreateDto.DriverId.ToString())?.Text;
            ViewBag.SelectedDriverName = selectedDriverName ?? string.Empty;
            ViewBag.SelectedDriverId = mechanicAcceptanceForCreateDto.DriverId;

            var selectedCar = cars.FirstOrDefault(c => c.Value == mechanicAcceptanceForCreateDto.CarId.ToString())?.Text;
            ViewBag.SelectedCar = selectedCar ?? string.Empty;
            ViewBag.SelectedCarId = mechanicAcceptanceForCreateDto.CarId;

            return View(mechanicAcceptanceForCreateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MechanicAcceptanceForUpdateDto mechanicAcceptance)
        {
            if (id != mechanicAcceptance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _mechanicAcceptanceDataStore.UpdateMechanicAcceptanceAsync(id, mechanicAcceptance);
                }
                catch (Exception)
                {
                    if (!await MechanicAcceptanceExists(id))
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

            return View(mechanicAcceptance);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var mechanicAcceptance = await _mechanicAcceptanceDataStore.GetMechanicAcceptanceAsync(id);
            if (mechanicAcceptance == null)
            {
                return NotFound();
            }
            return View(mechanicAcceptance);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mechanicAcceptanceDataStore.DeleteMechanicAcceptanceAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> MechanicAcceptanceExists(int id)
        {
            var mechanicAcceptance = await _mechanicAcceptanceDataStore.GetMechanicAcceptanceAsync(id);
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

        public async Task<IActionResult> GetCarByDriverId(int driverId)
        {
            var operatorReviews = await _operatorReviewDataStore.GetOperatorReviews(null, null, null);
            var operatorr = operatorReviews.Data.FirstOrDefault(m => m.DriverId == driverId && m.Date.Value.Date == DateTime.Today);

            if (operatorr != null)
            {
                var car = await _carDataStore.GetCarAsync(operatorr.CarId);
                return Json(new { success = true, car });
            }
            return Json(new { success = false });
        }
    }
}