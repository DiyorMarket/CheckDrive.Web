using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.MechanicHandover;
using CheckDrive.Web.Models;
using CheckDrive.Web.Stores.Cars;
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

        public MechanicHandoversController(IMechanicHandoverDataStore mechanicHandoverDataStore, IDriverDataStore driverDataStore, ICarDataStore carDataStore, IMechanicDataStore mechanicDataStore)
        {
            _mechanicHandoverDataStore = mechanicHandoverDataStore;
            _driverDataStore = driverDataStore;
            _carDataStore = carDataStore;
            _mechanicDataStore = mechanicDataStore;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            var response = await _mechanicHandoverDataStore.GetMechanicHandoversAsync(pageNumber);

            ViewBag.PageSize = response.PageSize;
            ViewBag.PageCount = response.TotalPages;
            ViewBag.TotalCount = response.TotalCount;
            ViewBag.CurrentPage = response.PageNumber;
            ViewBag.HasPreviousPage = response.HasPreviousPage;
            ViewBag.HasNextPage = response.HasNextPage;

            var mechanicHandovers = response.Data.Select(r => new
            {
                r.Id,
                IsHanded = r.IsHanded ? "Qabul qilindi" : "Rad etildi",
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
        public async Task<IActionResult> PersonalIndex(string? searchstring, int? pageNumber)
        {
            var drivers = await _driverDataStore.GetDriversAsync();
            var cars = await _carDataStore.GetCarsAsync(searchstring, pageNumber);
            var mechanics = await _mechanicDataStore.GetMechanicsAsync();
            var response = await _mechanicHandoverDataStore.GetMechanicHandoversAsync(pageNumber);

            var mechanicHandovers = response.Data
                .OrderByDescending(r => r.Date)
                .Select(r => new
                {
                    r.Id,
                    IsHanded = r.IsHanded ? "Qabul qilindi" : "Rad etildi",
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

            ViewBag.PageSize = response.PageSize;
            ViewBag.PageCount = response.TotalPages;
            ViewBag.TotalCount = response.TotalCount;
            ViewBag.CurrentPage = response.PageNumber;
            ViewBag.HasPreviousPage = response.HasPreviousPage;
            ViewBag.HasNextPage = response.HasNextPage;

            ViewBag.Mechanics = mechanics.Data.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = $"{d.FirstName} {d.LastName}"
            }).ToList();

            ViewBag.Drivers = drivers.Data.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = $"{d.FirstName} {d.LastName}"
            }).ToList();

            ViewBag.Cars = cars.Data.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = $"{c.Model} ({c.Number})"
            }).ToList();

            ViewBag.MechanicHandovers = mechanicHandovers;

            return View();
        }



        public async Task<IActionResult> Create()
        {
            var mechanics = await GETMechanics();
            var drivers = await GETDrivers();
            var cars = await GETCars();

            ViewBag.Mechanics = new SelectList(mechanics, "Value", "Text");
            ViewBag.Drivers = new SelectList(drivers, "Value", "Text");
            ViewBag.Cars = new SelectList(cars, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IsHanded,Comments,Date,MechanicId,Distance,CarId,DriverId")] MechanicHandoverForCreateDto mechanicHandoverForCreateDto)
        {
            if (ModelState.IsValid)
            {
                mechanicHandoverForCreateDto.Date = DateTime.Now;
                await _mechanicHandoverDataStore.CreateMechanicHandoverAsync(mechanicHandoverForCreateDto);
                return RedirectToAction(nameof(PersonalIndex));
            }
            return View(mechanicHandoverForCreateDto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var mechanicHandover = await _mechanicHandoverDataStore.GetMechanicHandoverAsync(id);
            if (mechanicHandover == null)
            {
                return NotFound();
            }

            ViewBag.Mechanics = new SelectList(await GETMechanics(), "Value", "Text");
            ViewBag.Drivers = new SelectList(await GETDrivers(), "Value", "Text");
            ViewBag.Cars = new SelectList(await GETCars(), "Value", "Text");

            return View(mechanicHandover);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IsHanded,Comments,Status,Date,MechanicId,CarId,DriverId")] MechanicHandoverForUpdateDto mechanicHandover)
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
            var mechanicHandover = await _mechanicHandoverDataStore.GetMechanicHandoverAsync(id);
            return mechanicHandover != null;
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
    }
}
