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
            var accounts = await _accountDataStore.GetAccountsAsync();
            ViewBag.Accounts = accounts.Data;
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var account = await _accountDataStore.GetAccountAsync(id);
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
                await _accountDataStore.CreateAccountAsync(account);
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var account = await _accountDataStore.GetAccountAsync(id);
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
                    await _accountDataStore.UpdateAccountAsync(id, account);
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
            var account = await _accountDataStore.GetAccountAsync(id);
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
            await _accountDataStore.DeleteAccountAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> AccountExists(int id)
        {
            var account = await _accountDataStore.GetAccountAsync(id);
            return account != null;
        }
    }
}
