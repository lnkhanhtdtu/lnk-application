using Lnk.Application.Abstracts;
using Lnk.UI.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Lnk.Application.Services;
using Microsoft.AspNetCore.Identity;
using Lnk.Domain.Entities;

namespace Lnk.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthenticationController(IAuthenticationService authenticationService, SignInManager<ApplicationUser> signInManager)
        {
            _authenticationService = authenticationService;
            _signInManager = signInManager;
        }
        // GET: AuthenticationController
        public ActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            var model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authenticationService.CheckLogin(model.Username, model.Password, model.RememberMe);
                if (result.Status)
                {
                    return RedirectToAction("Index", "HomeAdmin");
                }
                else
                {
                    TempData["ErrorMessage"] = result.Message;
                }
            }
            else
            {
                var error = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToString();
                TempData["ErrorMessage"] = string.Join("<br/> ", error);
            }
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return View("Login");
        }
    }
}