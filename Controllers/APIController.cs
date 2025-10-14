using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UNI_ASSETS.Data;
using UNI_ASSETS.Models;

namespace UNI_ASSETS.Controllers
{
    public class APIController : Controller
    {
        private readonly IRepositoryWrapper repository;

        public APIController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }
        [HttpPost]
        public  IActionResult Submit([FromBody] Submission submission)
        {
            submission.CreatedDate = DateTime.UtcNow;
            submission.Reviewed = false;
            submission.ReviewStatus = "Pending";

            repository.SubmissionRepository.Create(submission);
            repository.Save();

            return Ok(new { Message = "Submission received successfully!" });
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var submissions = repository.SubmissionRepository.GetAll().OrderByDescending(s => s.CreatedDate).ToList();
            return Ok(submissions);
        }
    }
}
