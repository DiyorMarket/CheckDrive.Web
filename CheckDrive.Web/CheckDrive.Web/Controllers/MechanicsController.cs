using CheckDrive.Web.Models;
using CheckDrive.Web.Stores.Mechanics;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class MechanicsController : Controller
    {
        private readonly IMechanicDataStore _mechanicDataStore;

        public MechanicsController(IMechanicDataStore mechanicDataStore)
        {
            _mechanicDataStore = mechanicDataStore;
        }
        public async Task<IActionResult> Index()
        {
            var mechanics = await _mechanicDataStore.GetMechanicsAsync();
            return View(mechanics);
        }
        public async Task<IActionResult> Details(int id)
        {
            var mechanic = await _mechanicDataStore.GetMechanicAsync(id);
            if (mechanic == null)
            {
                return NotFound();
            }
            return View(mechanic);
        }
        public IActionResult Create()
        {
            return View();
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId")] Mechanic mechanic)
        {
            if (ModelState.IsValid)
            {
                await _mechanicDataStore.CreateMechanicAsync(mechanic);
                return RedirectToAction(nameof(Index));
            }
            return View(mechanic);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var mechanic = await _mechanicDataStore.GetMechanicAsync(id);
            if (mechanic == null)
            {
                return NotFound();
            }
            return View(mechanic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId")] Mechanic mechanic)
        {
            if (id != mechanic.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _mechanicDataStore.UpdateMechanicAsync(id, mechanic);
                }
                catch (Exception)
                {
                    if (!await MechanicExists(id))
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
            return View(mechanic);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var mechanic = await _mechanicDataStore.GetMechanicAsync(id);
            if (mechanic == null)
            {
                return NotFound();
            }
            return View(mechanic);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mechanicDataStore.DeleteMechanicAsync(id);
            return RedirectToAction(nameof(Index));
        }
        private async Task<bool> MechanicExists(int id)
        {
            var mechanic = await _mechanicDataStore.GetMechanicAsync(id);
            return mechanic != null;
        }
    }
}
