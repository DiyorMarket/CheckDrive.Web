using CheckDrive.Web.Models;
using CheckDrive.Web.Stores.MechanicAcceptances;
using CheckDrive.Web.Stores.Mechanics;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class MechanicAcceptancesController : Controller
    {
        private readonly IMechanicAcceptanceDataStore _mechanicAcceptanceDataStore;

        public MechanicAcceptancesController(IMechanicAcceptanceDataStore mechanicAcceptanceDataStore, IMechanicDataStore mechanicDataStore)
        {
            _mechanicAcceptanceDataStore = mechanicAcceptanceDataStore;
        }

        public async Task<IActionResult> Index()
        {
            var mechanicAcceptances = await _mechanicAcceptanceDataStore.GetMechanicAcceptancesAsync();

            if (mechanicAcceptances == null)
            {
                return BadRequest();
            }

            ViewBag.MechanicAcceptances = mechanicAcceptances.Data;

            return View();
        }
        public async Task<IActionResult> Details(int id)
        {
            var mechanicAcceptance = await _mechanicAcceptanceDataStore.GetMechanicAcceptanceAsync(id);
            if (mechanicAcceptance == null)
            {
                return NotFound();
            }
            return View(mechanicAcceptance);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IsAccepted,Comments,Status,Date,Distance")] MechanicAcceptance mechanicAcceptance)
        {
            if (ModelState.IsValid)
            {
                await _mechanicAcceptanceDataStore.CreateMechanicAcceptanceAsync(mechanicAcceptance);
                return RedirectToAction(nameof(Index));
            }
            return View(mechanicAcceptance);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var mechanicAcceptance = await _mechanicAcceptanceDataStore.GetMechanicAcceptanceAsync(id);
            if (mechanicAcceptance == null)
            {
                return NotFound();
            }
            return View(mechanicAcceptance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IsAccepted,Comments,Status,Date,Distance")] MechanicAcceptance mechanicAcceptance)
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
    }
}
