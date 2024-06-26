using CheckDrive.ApiContracts.Account;
using CheckDrive.ApiContracts.Doctor;
using CheckDrive.Web.Stores.Doctors;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly IDoctorDataStore _doctorDataStore;

        public DoctorsController(IDoctorDataStore doctorDataStore)
        {
            _doctorDataStore = doctorDataStore;
        }

        public async Task<IActionResult> Index()
        {
            var doctors = await _doctorDataStore.GetDoctors();
            return View(doctors);
        }

        public async Task<IActionResult> Details(int id)
        {
            var doctor = await _doctorDataStore.GetDoctor(id);
            if (doctor == null)
            {
                return NotFound();
            }
            return View(doctor);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId")] DoctorForCreateDto doctor)
        {
            if (ModelState.IsValid)
            {
                await _doctorDataStore.CreateDoctor(doctor);
                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var doctor = await _doctorDataStore.GetDoctor(id);
            if (doctor == null)
            {
                return NotFound();
            }
            return View(doctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId")] AccountForUpdateDto doctor)
        {
            if (id != doctor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _doctorDataStore.UpdateDoctor(id, doctor);
                }
                catch (Exception)
                {
                    if (!await DoctorExists(id))
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
            return View(doctor);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _doctorDataStore.GetDoctor(id);
            if (doctor == null)
            {
                return NotFound();
            }
            return View(doctor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _doctorDataStore.DeleteDoctor(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> DoctorExists(int id)
        {
            var doctor = await _doctorDataStore.GetDoctor(id);
            return doctor != null;
        }
    }
}
