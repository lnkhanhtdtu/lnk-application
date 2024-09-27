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

        public AccountController(IUserServices userServices)
        {
            _userServices = userServices;
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AccountDTO account)
        {
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
