using CheckDrive.ApiContracts.Car;
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

        public async Task<IActionResult> Index()
        {
            var cars = await _carDataStore.GetCars();

            ViewBag.Cars = cars.Data;
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
            if (id != car.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _carDataStore.UpdateCar(id, car);
                }
                catch (Exception)
                {
                    if (!await CarExists(id))
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

        private async Task<bool> CarExists(int id)
        {
            var car = await _carDataStore.GetCar(id);
            return car != null;
        }
    }
}
