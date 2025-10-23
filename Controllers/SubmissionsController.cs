using Microsoft.AspNetCore.Mvc;
using UNI_ASSETS.Data;
using UNI_ASSETS.Models;

namespace UNI_ASSETS.Controllers
{
    public class SubmissionsController : Controller
    {
        private readonly IRepositoryWrapper repository;

        public SubmissionsController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }
        [HttpPost]
        public  IActionResult Reject(int id)
        {
            var submission = repository.SubmissionRepository.GetById(id);
            if (submission == null) return NotFound();

            submission.ReviewStatus = ReviewStatus.Rejected;
           // submission.Reviewed = true;
            submission.DateReviewed = DateTime.Now;

            repository.SubmissionRepository.Update(submission);
            repository.Save();

            TempData["Message"] = $"Submission #{id} rejected.";
            return RedirectToAction(nameof(ReviewList));
        }

        // Optional: View submission details
        public  IActionResult Details(int id)
        {
            var submission = repository.SubmissionRepository.GetById(id);

            if (submission == null)
                return NotFound();

            return View(submission);
        }

        public IActionResult ReviewList()
        {
            var submissions = repository.SubmissionRepository.GetSubmissionsWithDetails();
            return View(submissions);
        }
    }
}
