using CheckDrive.ApiContracts.OilMark;
using CheckDrive.Web.Stores.OilMarks;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class OilMarksController : Controller
    {
        private readonly IOilMarkDataStore _oilMarkDataStore;

        public OilMarksController(IOilMarkDataStore oilMarkDataStore)
        {
            _oilMarkDataStore = oilMarkDataStore;
        }
        public async Task<IActionResult> Index()
        {
            var oilMarks = await _oilMarkDataStore.GetOilMarksAsync();
            return View(oilMarks);
        }

        public async Task<IActionResult> Details(int id)
        {
            var oilMark = await _oilMarkDataStore.GetOilMarkByIdAsync(id);
            if (oilMark == null)
            {
                return NotFound();
            }
            return View(oilMark);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OilMark")] OilMarkForCreateDto oilMark)
        {
            if (ModelState.IsValid)
            {
                await _oilMarkDataStore.CreateOilMarkAsync(oilMark);
                return RedirectToAction(nameof(Index));
            }
            return View(oilMark);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var oilMark = await _oilMarkDataStore.GetOilMarkByIdAsync(id);
            if (oilMark == null)
            {
                return NotFound();
            }
            return View(oilMark);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OilMark")] OilMarkForUpdateDto oilMark)
        {
            if (ModelState.IsValid)
            {
                await _oilMarkDataStore.UpdateOilMarkAsync(id, oilMark);
                return RedirectToAction(nameof(Index));
            }
            return View(oilMark);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var oilMark = await _oilMarkDataStore.GetOilMarkByIdAsync(id);
            if (oilMark == null)
            {
                return NotFound();
            }
            return View(oilMark);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _oilMarkDataStore.DeleteOilMarkAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
