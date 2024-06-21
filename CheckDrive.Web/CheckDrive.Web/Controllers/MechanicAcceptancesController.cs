using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.Web.Stores.Cars;
using CheckDrive.Web.Stores.Drivers;
using CheckDrive.Web.Stores.MechanicAcceptances;
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

        public MechanicAcceptancesController(IMechanicAcceptanceDataStore mechanicAcceptanceDataStore, IDriverDataStore driverDataStore, ICarDataStore carDataStore, IMechanicDataStore mechanicDataStore, IOperatorReviewDataStore operatorReviewDataStore)
        {
            _mechanicAcceptanceDataStore = mechanicAcceptanceDataStore;
            _driverDataStore = driverDataStore;
            _carDataStore = carDataStore;
            _mechanicDataStore = mechanicDataStore;
            _operatorReviewDataStore = operatorReviewDataStore;
        }

        public async Task<IActionResult> Index(int? pageNumber, string? searchString)
        {

            var response = await _mechanicAcceptanceDataStore.GetMechanicAcceptancesAsync(pageNumber, searchString);

            ViewBag.PageSize = response.PageSize;
            ViewBag.PageCount = response.TotalPages;
            ViewBag.TotalCount = response.TotalCount;
            ViewBag.CurrentPage = response.PageNumber;
            ViewBag.HasPreviousPage = response.HasPreviousPage;
            ViewBag.HasNextPage = response.HasNextPage;

            var mechanicAcceptances = response.Data.Select(r => new
            {
                r.Id,
                IsAccepted = r.IsAccepted ? "Qabul qilindi" : "Rad etildi",
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
            var response = await _mechanicAcceptanceDataStore.GetMechanicAcceptancesAsync(null, null);
            var operatorReviewsResponse = await _operatorReviewDataStore.GetOperatorReviews(null, searchString);

            var filteredOperatorReviews = operatorReviewsResponse.Data
                .Where(dr => dr.Date == DateTime.Today)
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
                            Date = review.Date
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
                            Date = null
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

            return View(mechanicAcceptance);
        }

        public async Task<IActionResult> Create(int? driverId)
        {
            var mechanics = await GETMechanics();
            var drivers = await GETDrivers();
            var cars = await GETCars();

            var accountIdStr = TempData["AccountId"] as string;
            TempData.Keep("AccountId");

            if (int.TryParse(accountIdStr, out int accountId))
            {
                var mechanicResponse = await _mechanicDataStore.GetMechanics(accountId);
                var mechanic = mechanicResponse.Data.First();
                if (mechanic != null)
                {
                    var filteredDrivers = drivers
                        .Where(d => d.Value == driverId.ToString())
                        .ToList();

                    ViewBag.Mechanics = new SelectList(mechanics, "Value", "Text");
                    ViewBag.Drivers = new SelectList(filteredDrivers, "Value", "Text", driverId);
                    ViewBag.Cars = new SelectList(cars, "Value", "Text");

                    var selectedDriverName = filteredDrivers.FirstOrDefault(d => d.Value == driverId.ToString())?.Text;
                    ViewBag.SelectedDriverName = selectedDriverName ?? string.Empty;
                    ViewBag.SelectedDriverId = driverId;

                    return View(new MechanicAcceptanceForCreateDto { DriverId = driverId ?? 0, MechanicId = mechanic.Id });
                }
            }

            return NotFound("Механик не найден для указанного аккаунта.");
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
            ViewBag.Cars = new SelectList(cars, "Value", "Text");

            var selectedDriverName = drivers.FirstOrDefault(d => d.Value == mechanicAcceptanceForCreateDto.DriverId.ToString())?.Text;
            ViewBag.SelectedDriverName = selectedDriverName ?? string.Empty;
            ViewBag.SelectedDriverId = mechanicAcceptanceForCreateDto.DriverId;

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
    }
}