using Microsoft.AspNetCore.Mvc;
using UNI_ASSETS.Models.ViewModels;

namespace UNI_ASSETS.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            return View();
        }
    }
}
