using Microsoft.AspNetCore.Mvc;
using UNI_ASSETS.Data;
using UNI_ASSETS.Models.ViewModels;

namespace UNI_ASSETS.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepositoryWrapper repository;

        public HomeController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }
        [TempData]
        public string State { get; set; }
        [HttpPost]
        public IActionResult StartTimer(string id = "OFF")
        {
           
            if(id == "OFF")
            {
                id = "ON";
                repository.AnalyticsRepository.ChangeInterval(100000);
                repository.AnalyticsRepository.Start();

            }
            else
            {
                id = "OFF";
                repository.AnalyticsRepository.Stop();
            }
            State = id;
                return RedirectToAction("Index");
        }
        public IActionResult Index()
        {
            var model = new HomeViewModel { Assets = repository.AssetRepository.GetAll().ToList(), Submissions = null };
            State = "OFF";
            return View(model);
        }
        
    }
}
