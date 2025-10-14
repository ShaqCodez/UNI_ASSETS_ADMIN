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
        public IActionResult Index()
        {
            var model = new HomeViewModel { Assets = repository.AssetRepository.GetAll().ToList(), Submissions = null };
            return View(model);
        }
    }
}
