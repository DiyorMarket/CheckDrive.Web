using CheckDrive.ApiContracts.OilMark;
using CheckDrive.Web.Stores.OilMarks;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class OilMarksController(IOilMarkDataStore oilMarkDataStore) : Controller
    {
        private readonly IOilMarkDataStore _oilMarkDataStore = oilMarkDataStore;

        public async Task<IActionResult> Index()
        {
            var oilMarks = await _oilMarkDataStore.GetOilMarksAsync();
            var oilMarkss = oilMarks.Data.ToList();
            return View(oilMarkss);
        }

        public async Task<IActionResult> Details(int id)
        {
            var oilMark = await _oilMarkDataStore.GetOilMarkByIdAsync(id);

            return View(oilMark);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OilMark")] OilMarkForCreateDto _oilmark)
        {
            if (ModelState.IsValid)
            {
                await _oilMarkDataStore.CreateOilMarkAsync(_oilmark);
                return RedirectToAction(nameof(Index));
            }
            return View(_oilmark);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var oilMark = await _oilMarkDataStore.GetOilMarkByIdAsync(id);

            return View(oilMark);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OilMark")] OilMarkForUpdateDto _oilMark)
        {
            if (ModelState.IsValid)
            {
                await _oilMarkDataStore.UpdateOilMarkAsync(id, _oilMark);
                return RedirectToAction(nameof(Index));
            }
            return View(_oilMark);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var oilMark = await _oilMarkDataStore.GetOilMarkByIdAsync(id);
  
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
