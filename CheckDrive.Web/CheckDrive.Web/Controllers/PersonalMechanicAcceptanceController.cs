using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.Web.Stores.Drivers;
using CheckDrive.Web.Stores.MechanicAcceptances;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class PersonalMechanicAcceptanceController : Controller
    {
        private readonly IMechanicAcceptanceDataStore _mechanicAcceptanceDataStore;
        private readonly IDriverDataStore _driverDataStore;
        public PersonalMechanicAcceptanceController(IMechanicAcceptanceDataStore mechanicAcceptanceDataStore, IDriverDataStore driverDataStore)
        {
            _mechanicAcceptanceDataStore = mechanicAcceptanceDataStore;
            _driverDataStore = driverDataStore;
        }
        public async Task<IActionResult> Index()
        {
            var drivers = await _driverDataStore.GetDriversAsync();
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
            ViewBag.MechanicAcceptances = mechanicAcceptances;

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
    } 
}
