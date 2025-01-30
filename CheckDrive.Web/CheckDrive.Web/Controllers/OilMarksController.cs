using CheckDrive.Web.Requests.OilMark;
using CheckDrive.Web.Stores.OilMarks;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers;

public class OilMarksController(IOilMarkStore oilMarkDataStore) : Controller
{
    public async Task<IActionResult> Index()
    {
        var oilMarks = await oilMarkDataStore.GetAsync();

        return View(oilMarks);
    }

    public async Task<IActionResult> Details(int id)
    {
        var oilMark = await oilMarkDataStore.GetByIdAsync(id);

        return View(oilMark);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] CreateOilMarkRequest request)
    {
        if (ModelState.IsValid)
        {
            await oilMarkDataStore.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }
        return View(request);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var oilMark = await oilMarkDataStore.GetByIdAsync(id);

        return View(oilMark);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromForm] UpdateOilMarkRequest request)
    {
        if (ModelState.IsValid)
        {
            await oilMarkDataStore.UpdateAsync(request);
            return RedirectToAction(nameof(Index));
        }
        return View(request);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var oilMark = await oilMarkDataStore.GetByIdAsync(id);

        return View(oilMark);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await oilMarkDataStore.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
