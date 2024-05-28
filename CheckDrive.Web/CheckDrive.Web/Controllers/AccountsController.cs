using CheckDrive.ApiContracts.Account;
using CheckDrive.ApiContracts.Role;
using CheckDrive.Web.Stores.Accounts;
using CheckDrive.Web.Stores.Roles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var accounts = await _accountDataStore.GetAccountsAsync(searchString, roleId, birthDate);

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
        public async Task<IActionResult> Details(int id)
        {
            var account = await _accountDataStore.GetAccountAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }
        public async Task<IActionResult> Create()
        {
            var roles = await GETRoles();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Login,Password,PhoneNumber,FirstName,LastName,Bithdate,RoleId")]
            AccountForCreateDto account)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Login,Password,PhoneNumber,FirstName,LastName,Bithdate,RoleId")] 
            AccountForUpdateDto account)
        {
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
        private async Task<List<RoleDto>> GETRoles()
        {
            var roleResponse = await _roleStore.GetRoles();
            var roles = roleResponse.Data.ToList();
            return roles;
        }
    }
}
