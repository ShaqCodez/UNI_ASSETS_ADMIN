using Microsoft.AspNetCore.Mvc;

namespace UNI_ASSETS.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
