using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.Web.Stores.Cars;
using CheckDrive.Web.Stores.Drivers;
using CheckDrive.Web.Stores.MechanicAcceptances;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckDrive.Web.Controllers
{
    public class PersonalMechanicAcceptanceController : Controller
    {
        private readonly IMechanicAcceptanceDataStore _mechanicAcceptanceDataStore;
        private readonly IDriverDataStore _driverDataStore;
        private readonly ICarDataStore _carDataStore;
        public PersonalMechanicAcceptanceController(IMechanicAcceptanceDataStore mechanicAcceptanceDataStore, IDriverDataStore driverDataStore, ICarDataStore carDataStore)
        {
            _mechanicAcceptanceDataStore = mechanicAcceptanceDataStore;
            _driverDataStore = driverDataStore;
            _carDataStore = carDataStore;
        }
        public async Task<IActionResult> Index()
        {
            var drivers = await _driverDataStore.GetDriversAsync();
            var cars = await _carDataStore.GetCarsAsync();
            var response = await _mechanicAcceptanceDataStore.GetMechanicAcceptancesAsync();

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

            ViewBag.Drivers = drivers;
            ViewBag.Cars = cars;
            ViewBag.MechanicAcceptances = mechanicAcceptances;

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
        public async Task<IActionResult> Create(MechanicAcceptanceForCreateDto mechanicAcceptance)
        {
            if (ModelState.IsValid)
            {
                await _mechanicAcceptanceDataStore.CreateMechanicAcceptanceAsync(mechanicAcceptance);
                return RedirectToAction(nameof(Index));
            }
            return View(mechanicAcceptance);
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
            var doctors = carResponse.Data
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = $"{d.Model} ({d.Number})"
                })
                .ToList();
            return doctors;
        }
    } 
}
