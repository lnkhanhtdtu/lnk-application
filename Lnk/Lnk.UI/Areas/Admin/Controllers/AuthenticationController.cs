using Lnk.UI.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lnk.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthenticationController : Controller
    {
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
        public IActionResult Login(LoginModel model)
        {
            return View(model);
        }
    }
}
