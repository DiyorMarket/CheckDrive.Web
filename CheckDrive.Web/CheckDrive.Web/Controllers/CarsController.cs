using CheckDrive.Web.Mappings;
using CheckDrive.Web.Requests.Cars;
using CheckDrive.Web.Stores.Cars;
using CheckDrive.Web.Stores.OilMarks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckDrive.Web.Controllers;

public class CarsController(ICarStore carStore, IOilMarkStore oilMarkStore) : Controller
{
    public async Task<IActionResult> Index()
    {
        var cars = await carStore.GetAsync();

        ViewBag.Cars = cars;
        return View();
    }

    public async Task<IActionResult> Details(int id)
    {
        var car = await carStore.GetByIdAsync(id);

        return View(car);
    }

    public async Task<IActionResult> DetailsForMechanicAcceptance(int id)
    {
        var car = await carStore.GetByIdAsync(id);
        if (car == null)
        {
            return NotFound();
        }
        return PartialView("_CarDetailsForMechanicAcceptance", car);
    }

    public async Task<IActionResult> DetailsForMechanicHandover(int id)
    {
        var car = await carStore.GetByIdAsync(id);
        if (car == null)
        {
            return NotFound();
        }
        return PartialView("_CarDetailsForMechanicHandover", car);
    }

    public async Task<IActionResult> Create()
    {
        var oilMarks = await GetOilMarkAsync();
        ViewBag.OilMarks = oilMarks;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] CreateCarRequest request)
    {
        if (ModelState.IsValid)
        {
            var newCar = await carStore.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }
        return BadRequest(ModelState);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var car = await carStore.GetByIdAsync(id);
        var oilMarks = await GetOilMarkAsync();

        ViewBag.OilMarks = oilMarks;
        var carRequest = car.ToUpdateViewModel();

        return View(carRequest);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromForm] UpdateCarRequest request)
    {
        try
        {
            await carStore.UpdateAsync(request);
            return RedirectToAction(nameof(Index));
        }
        catch(Exception ex)
        {
            return BadRequest();
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        var car = await carStore.GetByIdAsync(id);
        if (car == null)
        {
            return NotFound();
        }
        return View(car);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await carStore.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task<List<SelectListItem>> GetOilMarkAsync()
    {
        var oilMarks = await oilMarkStore.GetAsync();

        return oilMarks
            .Select(e => new SelectListItem()
            {
                Value = e.Id.ToString(),
                Text = e.Name.ToString()
            })
            .ToList();
    }
}