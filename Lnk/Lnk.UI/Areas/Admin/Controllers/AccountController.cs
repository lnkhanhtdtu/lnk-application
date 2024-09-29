using Lnk.Application.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lnk.Application.DTOs;
using Lnk.Application.Services;
namespace Lnk.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    // [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IRoleService _roleService;

        public AccountController(IUserServices userServices, IRoleService roleService)
        {
            _userServices = userServices;
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAccountPagination(RequestDataTable request)
        {
            var response = await _userServices.GetUserPagination(request);
            return Json(response);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string? id)
        {
            var accountDTO = string.IsNullOrEmpty(id) 
            ? new AccountDTO() 
            : await _userServices.GetUserById(id);

            var role = await _roleService.GetRoleForDropdownList();
            ViewBag.Role = role;

            return View(accountDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccountDTO account)
        {
            var role = await _roleService.GetRoleForDropdownList();
            ViewBag.Role = role;

            if (ModelState.IsValid)
            {
                var response = await _userServices.CreateUser(account);
                if (response.Status)
                {
                    return RedirectToAction("Index", "Account");
                }

                ModelState.AddModelError("errorModel", response.Message);
            }
            else
            {
                ModelState.AddModelError("errorModel", "Có lỗi xảy ra");
            }

            return View(account);
        }

        public IActionResult Edit()
        {
            return Ok("Sửa");
        }

        public IActionResult Delete()
        {
            return Ok("Xóa");
        }
    }
}
