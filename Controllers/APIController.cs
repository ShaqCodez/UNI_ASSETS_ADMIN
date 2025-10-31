using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
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
        /*
         Create endpoint for mobile app Logins;
         */
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IRepositoryWrapper repository;

        public APIController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IRepositoryWrapper repository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid data format." });

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
                return Unauthorized(new { message = "Invalid username or password." });

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
                return Unauthorized(new { message = "Invalid username or password." });

            
            return Ok(new
            {
                success = true,
                message = "User authenticated successfully.",
                username = user.UserName,
                email = user.Email
            });
        }
        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}

