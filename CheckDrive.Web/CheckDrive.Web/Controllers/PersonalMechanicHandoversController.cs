using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.MechanicHandover;
using CheckDrive.Web.Stores.Cars;
using CheckDrive.Web.Stores.Drivers;
using CheckDrive.Web.Stores.MechanicHandovers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckDrive.Web.Controllers
{
    public class PersonalMechanicHandoversController : Controller
    {
        private readonly IMechanicHandoverDataStore _mechanicHandoverDataStore;
        private readonly IDriverDataStore _driverDataStore;
        private readonly ICarDataStore _carDataStore;

        public PersonalMechanicHandoversController(IMechanicHandoverDataStore mechanicHandoverDataStore, IDriverDataStore driverDataStore, ICarDataStore carDataStore)
        {
            _mechanicHandoverDataStore = mechanicHandoverDataStore;
            _driverDataStore = driverDataStore;
            _carDataStore = carDataStore;
        }

        public async Task<IActionResult> Index()
        {
            var drivers = await _driverDataStore.GetDriversAsync();
            var cars = await _carDataStore.GetCarsAsync();
            var response = await _mechanicHandoverDataStore.GetMechanicHandoversAsync();

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
            var cars = await GETCars();
            var drivers = await GETDrivers();

            ViewBag.Drivers = new SelectList(drivers, "Value", "Text");
            ViewBag.Cars = new SelectList(cars, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MechanicHandoverForCreateDto mechanicHandover)
        {
            if (ModelState.IsValid)
            {
                await _mechanicHandoverDataStore.CreateMechanicHandoverAsync(mechanicHandover);

                TempData["SuccessMessage"] = "Ro'yxat muvaffaqiyatli yaratildi!";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Drivers = new SelectList(await GETDrivers(), "Value", "Text");
            ViewBag.Cars = new SelectList(await GETCars(), "Value", "Text");

            return View(mechanicHandover);
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
            var carResponse = await _carDataStore.GetCarsAsync();
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
