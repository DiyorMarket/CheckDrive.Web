using CheckDrive.ApiContracts.Car;
using CheckDrive.Web.Models;
using CheckDrive.Web.Stores.Cars;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarDataStore _carDataStore;

        public CarsController(ICarDataStore carDataStore)
        {
            _carDataStore = carDataStore;
        }

        public async Task<IActionResult> Index(string? searchString,int? pageNumber)
        {
            var cars = await _carDataStore.GetCarsAsync(searchString,pageNumber);

            ViewBag.SearchString = searchString;
            ViewBag.Cars = cars.Data;

            ViewBag.PageSize = cars.PageSize;
            ViewBag.PageCount = cars.TotalPages;
            ViewBag.TotalCount = cars.TotalCount;
            ViewBag.CurrentPage = cars.PageNumber;
            ViewBag.HasPreviousPage = cars.HasPreviousPage;
            ViewBag.HasNextPage = cars.HasNextPage;
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var car = await _carDataStore.GetCar(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        public async Task<IActionResult> DetailsForMechanicAcceptance(int id)
        {
            var car = await _carDataStore.GetCar(id);
            if (car == null)
            {
                return NotFound();
            }
            return PartialView("_CarDetailsForMechanicAcceptance", car);
        }

        public async Task<IActionResult> DetailsForMechanicHandover(int id)
        {
            var car = await _carDataStore.GetCar(id);
            if (car == null)
            {
                return NotFound();
            }
            return PartialView("_CarDetailsForMechanicHandover", car);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Model,Color,Number,MeduimFuelConsumption,FuelTankCapacity,ManufacturedYear")] CarForCreateDto car)
        {
            if (ModelState.IsValid)
            {
                await _carDataStore.CreateCar(car);
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var car = await _carDataStore.GetCar(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Model,Color,Number,MeduimFuelConsumption,FuelTankCapacity,ManufacturedYear")] CarForUpdateDto car)
        {
            if (ModelState.IsValid)
            {
                await _carDataStore.UpdateCar(id, car);
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var car = await _carDataStore.GetCar(id);
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
            await _carDataStore.DeleteCar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
