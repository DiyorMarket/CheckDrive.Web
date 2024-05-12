using CheckDrive.Web.Models;
using CheckDrive.Web.Stores.Technicians;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class TechniciansController : Controller
    {
        private readonly ITechnicianDataStore _technicianDataStore;

        public TechniciansController(ITechnicianDataStore technicianDataStore)
        {
            _technicianDataStore = technicianDataStore;
        }
        public async Task<IActionResult> Index()
        {
            var technicians = await _technicianDataStore.GetTechnicians();
            return View(technicians);
        }

        public async Task<IActionResult> Details(int id)
        {
            var technician = await _technicianDataStore.GetTechnician(id);
            if (technician == null)
            {
                return NotFound();
            }
            return View(technician);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId")] Technician technician)
        {
            if (ModelState.IsValid)
            {
                await _technicianDataStore.CreateTechnician(technician);
                return RedirectToAction(nameof(Index));
            }
            return View(technician);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var technician = await _technicianDataStore.GetTechnician(id);
            if (technician == null)
            {
                return NotFound();
            }
            return View(technician);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, AccountId")] Technician technician)
        {
            if (id != technician.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _technicianDataStore.UpdateTechnician(id, technician);
                return RedirectToAction(nameof(Index));
            }
            return View(technician);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var technician = await _technicianDataStore.GetTechnician(id);
            if (technician == null)
            {
                return NotFound();
            }
            return View(technician);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _technicianDataStore.DeleteTechnician(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
