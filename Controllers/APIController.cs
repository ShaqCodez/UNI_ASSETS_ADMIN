using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UNI_ASSETS.Data;
using UNI_ASSETS.DTOs;
using UNI_ASSETS.Models;

namespace UNI_ASSETS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class APIController : Controller
    {
        private readonly IRepositoryWrapper repository;
        public APIController(IRepositoryWrapper repository)
        {
            this.repository = repository;
          
        }

        //[HttpPost]
        //public  IActionResult Submit([FromBody] Submission submission)
        //{
        //    submission.CreatedDate = DateTime.UtcNow;

        //    submission.ReviewStatus = ReviewStatus.Pending;

        //    repository.SubmissionRepository.Create(submission);
        //    repository.Save();

        //    return Ok(new { Message = "Submission received successfully!" });
        //}
        [HttpPost("submit")]
        public IActionResult Submit([FromBody] SubmissionDto dto)
        {
            if (dto == null)
                return BadRequest(new { Message = "Invalid submission payload." });

            try
            {
                // Convert Base64 image to byte array (if provided)
                byte[] imageBytes = null;
                if (!string.IsNullOrEmpty(dto.ImageBase64))
                {
                    try
                    {
                        imageBytes = Convert.FromBase64String(dto.ImageBase64);
                    }
                    catch (FormatException)
                    {
                        return BadRequest(new { Message = "Invalid Base64 image format." });
                    }
                }

                // Convert or fallback to UTC time
                DateTime parsedDate = DateTime.UtcNow;
                DateTime.TryParse(dto.Date, out parsedDate);
                var staffID = repository.StaffRepository.GetByCondition(x=>x.UserName == dto.StaffId).Id;
                // Create submission entity
                var submission = new Submission
                {
                    AssetId = dto.AssetId,
                    StaffId = staffID,
                    Condition = dto.Condition,
                    Note = dto.Note,
                    Location = dto.Location,
                    Image = imageBytes, // <--- directly store the bytes
                    CreatedDate = parsedDate,
                    ReviewStatus = ReviewStatus.Pending
                };

                repository.SubmissionRepository.Create(submission);
                repository.Save();

                return Ok(new
                {
                    Message = "Submission received successfully!",
                    submission.AssetId,
                    submission.StaffId,
                    submission.ReviewStatus
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while processing your submission.",
                    Error = ex.Message
                });
            }
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var submissions = repository.SubmissionRepository.GetAll().OrderByDescending(s => s.CreatedDate).ToList();
            return Ok(submissions);
        }
    }
}
