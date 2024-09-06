﻿using CheckDrive.ApiContracts.Account;
using CheckDrive.ApiContracts.Role;
using CheckDrive.Web.Stores.Accounts;
using CheckDrive.Web.Stores.Debts;
using CheckDrive.Web.Stores.Drivers;
using CheckDrive.Web.Stores.Roles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Principal;

namespace CheckDrive.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountDataStore _accountDataStore;
        private readonly IRoleDataStore _roleStore;
        private readonly IDriverDataStore _driverDataStore;
        private readonly IDebtDataStore _debtDataStore;
        public AccountsController(IAccountDataStore accountDataStore,
            IRoleDataStore roleDataStore,
            IDriverDataStore driverDataStore,
            IDebtDataStore debtDataStore)
        {
            _roleStore = roleDataStore;
            _accountDataStore = accountDataStore;
            _driverDataStore = driverDataStore;
            _debtDataStore = debtDataStore;
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
            var debts = await _debtDataStore.GetDebtsAsync(account.Id);

            ViewBag.Debts = debts;

            switch (account.RoleId)
            {
                case 2:
                    var driverHistories = await _driverDataStore.GetDriverHistories(id);
                    ViewBag.DriverHistories = driverHistories;
                    return View(account);
                case 3:
                    var doctorHistories = await _accountDataStore.GetDoctorHistories(id);
                    ViewBag.DoctorHistories = doctorHistories;
                    return View(account);
                case 4:
                    var operatorHistories = await _accountDataStore.GetOperatorHistories(id);
                    ViewBag.OperatorHistories = operatorHistories;
                    return View(account);
                case 5:
                    var dispatcherHistories = await _accountDataStore.GetDispatcherHistories(id);
                    ViewBag.DispatcherHistories = dispatcherHistories;
                    return View(account);
                case 6:
                    var mechanicHistories = await _accountDataStore.GetMechanicHistories(id);
                    ViewBag.MechanicHistories = mechanicHistories;
                    return View(account);
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
        public async Task<IActionResult> Create([Bind("Login,Password,PhoneNumber,FirstName,LastName,Address,Position,Passport,Bithdate,RoleId")]
            AccountForCreateDto account)
        {
            if (ModelState.IsValid)
            {
                var newAccount = await _accountDataStore.CreateAccountAsync(account);
                return RedirectToAction("Details", new {id = newAccount.Id});
            }
            var roles = await GETRoles();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View(account);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var account = await _accountDataStore.GetAccountAsync(id);

            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Login,Password,PhoneNumber,FirstName,LastName,Bithdate,Address,Position,Passport,RoleId")]
            AccountForUpdateDto account)
        {
            if (ModelState.IsValid)
            {
                var newAccount = await _accountDataStore.UpdateAccountAsync(id, account);
                return RedirectToAction("Details", new { id = newAccount.Id });
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
