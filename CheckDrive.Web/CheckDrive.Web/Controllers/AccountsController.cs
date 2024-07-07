using CheckDrive.ApiContracts.Account;
using CheckDrive.ApiContracts.Role;
using CheckDrive.Web.Stores.Accounts;
using CheckDrive.Web.Stores.Drivers;
using CheckDrive.Web.Stores.Roles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckDrive.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountDataStore _accountDataStore;
        private readonly IRoleDataStore _roleStore;
        private readonly IDriverDataStore _driverDataStore;
        public AccountsController(IAccountDataStore accountDataStore,
            IRoleDataStore roleDataStore,
            IDriverDataStore driverDataStore)
        {
            _roleStore = roleDataStore;
            _accountDataStore = accountDataStore;
            _driverDataStore = driverDataStore;
        }

        public async Task<IActionResult> Index(string? searchString, int? roleId, DateTime? birthDate, int? pageNumber)
        {
            var accounts = await _accountDataStore.GetAccountsAsync(searchString, roleId, birthDate, pageNumber);

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
            ViewBag.Roles = roles;

            ViewBag.PageSize = accounts.PageSize;
            ViewBag.PageCount = accounts.TotalPages;
            ViewBag.TotalCount = accounts.TotalCount;
            ViewBag.CurrentPage = accounts.PageNumber;
            ViewBag.HasPreviousPage = accounts.HasPreviousPage;
            ViewBag.HasNextPage = accounts.HasNextPage;

            ViewBag.SearchString = searchString;
            ViewBag.CurrentRoleId = roleId;
            ViewBag.SelectedRole = selectedRole;

            return View();
        }
        public async Task<IActionResult> Details(int id)
        {
            var account = await _accountDataStore.GetAccountAsync(id);  

            var accountHistories = await _driverDataStore.GetDriverHistories(id);

            var accounts = accountHistories.Select(r => new
            {
                r.Date,
                IsHanded = (bool)r.IsHanded ? "Berildi" : "Berilmadi",
                IsAccepted = (bool)r.IsAccepted ? "Qabul qilingan" : "Qabul qilinmagan",
                IsGiven = (bool)r.IsGiven ? "Quyildi" : "Quyilmadi",
                IsHealthy = (bool)r.IsHealthy ? "Sog`lom" : "Kasal",
                r.DoctorReviewId,
                r.MechanicAcceptanceId,
                r.MechanicHandoverId,
                r.OperatorReviewId,
            }).ToList();

            ViewBag.DriverHistories = accounts;

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
            var roles = await GETRoles();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
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
                await _accountDataStore.UpdateAccountAsync(id, account);
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

        private async Task<List<RoleDto>> GETRoles()
        {
            var roleResponse = await _roleStore.GetRoles();
            var roles = roleResponse.Data.ToList();
            return roles;
        }
    }
}
