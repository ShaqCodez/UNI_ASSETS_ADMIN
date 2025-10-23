using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UNI_ASSETS.Data;

namespace UNI_ASSETS.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ReportController : Controller
    {
        private readonly IRepositoryWrapper repository;

        public ReportController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }

        public IActionResult Details(string AssetId)
        {
            
            return View();
        }
    }
}
