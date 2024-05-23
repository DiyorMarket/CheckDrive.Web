using CheckDrive.ApiContracts.Account;
using CheckDrive.ApiContracts.Role;
using CheckDrive.Web.Stores.Accounts;
using CheckDrive.Web.Stores.Roles;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountDataStore _accountDataStore;
        private readonly IRoleDataStore _roleStore;
        public AccountsController(IAccountDataStore accountDataStore, IRoleDataStore roleDataStore)
        {
            _roleStore = roleDataStore;
            _accountDataStore = accountDataStore;
        }

        public async Task<IActionResult> Index(string? searchString, int? roleId, DateTime? birthDate)
        {
            var accounts = await _accountDataStore.GetAccounts(searchString, roleId, birthDate);

            var roles = await GETRoles();

            roles.Insert(0, new RoleDto
            {
                Id = 0,
                Name = "Barcha ishchilar",
            });
            var selectedRole = roles[0];

            if (roleId.HasValue && roleId != 0)
            {
                selectedRole = roles.FirstOrDefault(x => x.Id == roleId);
            }

            ViewBag.Accounts = accounts.Data;
            ViewBag.SearchString = searchString;

            ViewBag.Roles = roles;
            ViewBag.CurrentRoleId = roleId;
            ViewBag.SelectedRole = selectedRole;


            return View();
        }
        private async Task<List<RoleDto>> GETRoles()
        {
            var categoryResponse = await _roleStore.GetRoles();

            var categories = categoryResponse.Data.ToList();

            return categories;
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
        public async Task<IActionResult> Create([Bind("Login,Password,PhoneNumber,FirstName,LastName,Birthdate,RoleId")] AccountForCreateDto account)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Login,Password,PhoneNumber,FirstName,LastName,Birthdate,RoleId")] AccountForUpdateDto account)
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
