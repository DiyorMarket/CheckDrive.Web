using CheckDrive.Web.Models;
using CheckDrive.Web.Stores.Accounts;
using CheckDrive.Web.Stores.Drivers;
using CheckDrive.Web.Stores.Roles;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class DriversController : Controller
    {
        private readonly IDriverDataStore _driverDataStore;
        private readonly IAccountDataStore _accountDataStore;
        private readonly IRoleDataStore _roleDataStore;

        public DriversController(IDriverDataStore driverDataStore, IAccountDataStore accountDataStore, IRoleDataStore roleDataStore)
        {
            _driverDataStore = driverDataStore;
            _accountDataStore = accountDataStore;
            _roleDataStore = roleDataStore;
        }

        public async Task<IActionResult> Index()
        {
            var accounts = await _accountDataStore.GetAccounts();
            var roles = await _roleDataStore.GetRoles();
            var drivers = new List<Account>();

            foreach(var account in accounts.Data.ToList())
            {
                var role = roles.Data.ToList().FirstOrDefault(a=>a.Name.ToLower() == "driver");

                if(role.Id == account.RoleId)
                {
                    drivers.Add(account);
                }
            }

            ViewBag.Drivers = drivers;

            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var driver = await _driverDataStore.GetDriver(id);
            if (driver == null)
            {
                return NotFound();
            }
            return View(driver);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId")] Driver driver)
        {
            if (ModelState.IsValid)
            {
                await _driverDataStore.CreateDriver(driver);
                return RedirectToAction(nameof(Index));
            }
            return View(driver);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var driver = await _driverDataStore.GetDriver(id);
            if (driver == null)
            {
                return NotFound();
            }
            return View(driver);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId")] Driver driver)
        {
            if (id != driver.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _driverDataStore.UpdateDriver(id,driver);
                }
                catch (Exception)
                {
                    if (!await DriverExists(id))
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
            return View(driver);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var driver = await _driverDataStore.GetDriver(id);
            if (driver == null)
            {
                return NotFound();
            }
            return View(driver);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _driverDataStore.DeleteDriver(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> DriverExists(int id)
        {
            var driver = await _driverDataStore.GetDriver(id);
            return driver != null;
        }
    }
}
