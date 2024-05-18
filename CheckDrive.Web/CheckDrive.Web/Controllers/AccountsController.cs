using CheckDrive.Web.Models;
using CheckDrive.Web.Stores.Accounts;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountDataStore _accountDataStore;

        public AccountsController(IAccountDataStore accountDataStore)
        {
            _accountDataStore = accountDataStore;
        }

        public async Task<IActionResult> Index()
        {
            var accounts = await _accountDataStore.GetAccounts(2);
            return View(accounts);
        }

        public async Task<IActionResult> Details(int id)
        {
            var account = await _accountDataStore.GetAccount(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Login,Password,PhoneNumber,FirstName,LastName,Birthdate,RoleId")] Account account)
        {
            if (ModelState.IsValid)
            {
                await _accountDataStore.CreateAccount(account);
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var account = await _accountDataStore.GetAccount(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Login,Password,PhoneNumber,FirstName,LastName,Birthdate,RoleId")] Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _accountDataStore.UpdateAccount(id, account);
                }
                catch (Exception)
                {
                    if (!await AccountExists(id))
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
            return View(account);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var account = await _accountDataStore.GetAccount(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _accountDataStore.DeleteAccount(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> AccountExists(int id)
        {
            var account = await _accountDataStore.GetAccount(id);
            return account != null;
        }
    }
}
