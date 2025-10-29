using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using UNI_ASSETS.Data;
using UNI_ASSETS.Models;
using UNI_ASSETS.Models.ViewModels;

namespace UNI_ASSETS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IRepositoryWrapper repository;

        public HomeController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }
        
        public IActionResult Index()
        {
            var model = repository.AssetRepository.GetAll().ToList();
           
           
            return View(model);
        }
        
       

    }
}
