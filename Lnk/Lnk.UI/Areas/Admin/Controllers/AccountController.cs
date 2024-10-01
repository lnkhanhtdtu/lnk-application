using Lnk.Application.Abstracts;
using Lnk.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
namespace Lnk.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    // [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public AccountController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAccountPagination(RequestDataTable request)
        {
            var response = await _userService.GetUserPagination(request);
            return Json(response);
        }

        [HttpGet]
        public async Task<IActionResult> SaveData(string? id)
        {
            var accountDTO = string.IsNullOrEmpty(id) ? new AccountDTO() : await _userService.GetUserById(id);

            var role = await _roleService.GetRoleForDropdownList();
            ViewBag.Roles = role;

            return View(accountDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveData(AccountDTO accountDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _roleService.GetRoleForDropdownList();
                ModelState.AddModelError("errorsModel", "Invalid model");

                return View(accountDTO);
            }

            var result = await _userService.Save(accountDTO);

            if (result.Status)
            {
                return RedirectToAction("Index", "Account");
            }

            ModelState.AddModelError("errorsModel", result.Message);
            ViewBag.Roles = await _roleService.GetRoleForDropdownList();

            return View(accountDTO);
        }

        public IActionResult Delete()
        {
            return Ok("Xóa");
        }
    }
}
