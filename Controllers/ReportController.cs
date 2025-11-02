using iTextSharp.LGPLv2;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UNI_ASSETS.Data;
using UNI_ASSETS.Models;
using UNI_ASSETS.Models.ViewModels;
namespace UNI_ASSETS.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IRepositoryWrapper repository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ReportController(IRepositoryWrapper repository, IWebHostEnvironment webHostEnvironment)
        {
            this.repository = repository;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult GetAnalytics()
        {
            var analytics = GenerateAnalytics();
            return Ok(analytics);
        }

        /// <summary>
        /// Helper method to aggregate analytics from live data.
        /// </summary>
        private AppAnalytics GenerateAnalytics()
        {
            var submissions = repository.SubmissionRepository.GetSubmissionsWithDetails();

            // Build analytics dynamically from current submissions
            var analytics = new AppAnalytics
            {
                Submissions = submissions
            };

            return analytics;
        }
        [HttpGet]
        public IActionResult Reports()
        {
            var submissions = repository.SubmissionRepository.GetSubmissionsWithDetails();
            return View(submissions);
        }

        [HttpGet]
        public IActionResult FilterReports(string searchString, string sortOrder)
        {
            var submissions = repository.SubmissionRepository.GetSubmissionsWithDetails();

            // Filter
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                submissions = submissions.Where(s =>
                    (s.Asset != null && s.Asset.Name.ToLower().Contains(searchString)) ||
                    (s.Staff != null && s.Staff.UserName.ToLower().Contains(searchString)) ||
                    (!string.IsNullOrEmpty(s.Location) && s.Location.ToLower().Contains(searchString))
                ).ToList();
            }

            // Sort
            submissions = sortOrder switch
            {
                "name_asc" => submissions.OrderBy(s => s.Asset.Name).ToList(),
                "name_desc" => submissions.OrderByDescending(s => s.Asset.Name).ToList(),
                "location_asc" => submissions.OrderBy(s => s.Location).ToList(),
                "location_desc" => submissions.OrderByDescending(s => s.Location).ToList(),
                "status_asc" => submissions.OrderBy(s => s.ReviewStatus).ToList(),
                "status_desc" => submissions.OrderByDescending(s => s.ReviewStatus).ToList(),
                _ => submissions.OrderBy(s => s.Asset.AssetId).ToList()
            };

            // Return HTML snippet (partial view)
            return PartialView("_ReportTable", submissions);
        }
        public IActionResult Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Asset ID is required.");

            var asset = repository.AssetRepository.GetById(id);

            if (asset == null)
                return NotFound("Asset not found.");

            var submissions = repository.SubmissionRepository.GetAllAssetSubmissions(id).ToList();

            var analytics = new AppAnalytics
            {
                Submissions = submissions
            };

            var model = new AssetReportViewModel
            {
                Asset = asset,
                TotalReports = submissions.Count,
                PercentageReviewed = analytics.PercentageReviewed,
                PercentagePending = analytics.PercentagePending,
                PercentageRejected = analytics.PercentageRejected,
                AverageDuration = analytics.AverageDuration,
                Reports = submissions
                    .OrderByDescending(s => s.CreatedDate)
                    .ToList()
            };

            return View(model);
        }
        [HttpPost]
        public  IActionResult DownloadReport()
        {
            var submissions = repository.SubmissionRepository.GetSubmissionsWithDetails();

            using var stream = new MemoryStream();
            var doc = new Document(PageSize.A4, 25, 25, 25, 25);
            PdfWriter.GetInstance(doc, stream);
            doc.Open();
            AddLogo(doc, _webHostEnvironment.WebRootPath);
            // Title
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
            doc.Add(new Paragraph("Uni Assets Verification Report", titleFont));
            doc.Add(new Paragraph($"Generated on: {DateTime.Now}\n\n"));

            // Table
            PdfPTable table = new PdfPTable(6);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 15, 20, 20, 15, 15, 15 });

            AddCell(table, "Asset ID", true);
            AddCell(table, "Asset Name", true);
            AddCell(table, "Condition", true);
            AddCell(table, "Location", true);
            AddCell(table, "Staff", true);
            AddCell(table, "Review Status", true);

            foreach (var s in submissions)
            {
                AddCell(table, s.AssetId);
                AddCell(table, s.Asset?.Name ?? "N/A");
                AddCell(table, s.Condition ?? "N/A");
                AddCell(table, s.Location ?? "N/A");
                AddCell(table, s.Staff?.UserName ?? "N/A");
                AddCell(table, s.ReviewStatus.ToString());
            }

            doc.Add(table);
            doc.Close();

            return File(stream.ToArray(), "application/pdf", "UniAssetReport.pdf");
        }

        private void AddCell(PdfPTable table, string text, bool isHeader = false)
        {
            var font = isHeader
                ? FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)
                : FontFactory.GetFont(FontFactory.HELVETICA, 9);

            var cell = new PdfPCell(new Phrase(text, font))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = isHeader ? new BaseColor(230, 230, 250) : BaseColor.White
            };

            table.AddCell(cell);
        }
        [HttpGet]
        public IActionResult Index()
        {
            var model = GenerateReportViewModel();
            return View(model);
        }
        private ReportViewModel GenerateReportViewModel()
        {
            var submissions = repository.SubmissionRepository.GetAll().ToList();
            var analytics = new AppAnalytics { Submissions = submissions };

            return new ReportViewModel
            {
                TotalAssets = repository.AssetRepository.GetAll().Count(),
                TotalSubmissions = submissions.Count,
                PercentageReviewed = analytics.PercentageReviewed,
                PercentagePending = analytics.PercentagePending,
                PercentageRejected = analytics.PercentageRejected,
                AverageDuration = analytics.AverageDuration,
                ReviewBackLogWeekly = analytics.ReviewBackLogWeekly,
                ReviewBackLogMonthly = analytics.ReviewBackLogMonthly,
                ReviewStatusVelocity = analytics.ReviewStatusVelocity
            };
        }
        [HttpPost]
        public IActionResult DownloadAverageReviewTimeReport()
        {
            var submissions = repository.SubmissionRepository.GetSubmissionsWithDetails()
                .Where(s => s.DateReviewed != null)
                .ToList();

            using var stream = new MemoryStream();
            var doc = new Document(PageSize.A4, 25, 25, 25, 25);
            PdfWriter.GetInstance(doc, stream);
            doc.Open();
            AddLogo(doc, _webHostEnvironment.WebRootPath);
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
            doc.Add(new Paragraph("Average Review Time Report", titleFont));
            doc.Add(new Paragraph($"Generated on: {DateTime.Now}\n\n"));

            var avgDays = submissions.Average(s => (s.DateReviewed - s.CreatedDate)?.TotalDays ?? 0);
            doc.Add(new Paragraph($"Average Review Duration: {avgDays:F2} days\n\n"));

            PdfPTable table = new PdfPTable(3);
            table.WidthPercentage = 100;
            AddCell(table, "Asset ID", true);
            AddCell(table, "Created", true);
            AddCell(table, "Days to Review", true);

            foreach (var s in submissions)
            {
                AddCell(table, s.AssetId);
                AddCell(table, s.CreatedDate.ToString("yyyy-MM-dd"));
                AddCell(table, ((s.DateReviewed - s.CreatedDate)?.TotalDays ?? 0).ToString("F1"));
            }

            doc.Add(table);
            doc.Close();

            return File(stream.ToArray(), "application/pdf", "AverageReviewTimeReport.pdf");
        }


        [HttpPost]
        public IActionResult DownloadStatusDistributionReport()
        {
            var submissions = repository.SubmissionRepository.GetSubmissionsWithDetails();
            var grouped = submissions.GroupBy(s => s.ReviewStatus)
                .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
                .ToList();

            using var stream = new MemoryStream();
            var doc = new Document(PageSize.A4, 25, 25, 25, 25);
            PdfWriter.GetInstance(doc, stream);
            doc.Open();
            AddLogo(doc, _webHostEnvironment.WebRootPath);
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
            doc.Add(new Paragraph("Review Status Distribution Report", titleFont));
            doc.Add(new Paragraph($"Generated on: {DateTime.Now}\n\n"));

            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 60;
            AddCell(table, "Status", true);
            AddCell(table, "Count", true);

            foreach (var g in grouped)
            {
                AddCell(table, g.Status);
                AddCell(table, g.Count.ToString());
            }

            doc.Add(table);
            doc.Close();

            return File(stream.ToArray(), "application/pdf", "ReviewStatusDistribution.pdf");
        }


        [HttpPost]
        public IActionResult DownloadReportsByLocation()
        {
            var submissions = repository.SubmissionRepository.GetSubmissionsWithDetails();
            var grouped = submissions.GroupBy(s => s.Location ?? "Unknown")
                .Select(g => new { Location = g.Key, Count = g.Count() })
                .ToList();

            using var stream = new MemoryStream();
            var doc = new Document(PageSize.A4, 25, 25, 25, 25);
            PdfWriter.GetInstance(doc, stream);
            doc.Open();

            AddLogo(doc, _webHostEnvironment.WebRootPath);
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
            
            doc.Add(new Paragraph("Reports by Location", titleFont));
            
            doc.Add(new Paragraph($"Generated on: {DateTime.Now}\n\n"));

            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 80;
            AddCell(table, "Location", true);
            AddCell(table, "Total Reports", true);

            foreach (var g in grouped)
            {
                AddCell(table, g.Location);
                AddCell(table, g.Count.ToString());
            }

            doc.Add(table);
            doc.Close();

            return File(stream.ToArray(), "application/pdf", "ReportsByLocation.pdf");
        }
        private void AddLogo(Document doc, string wwwRootPath)
        {
            string logoPath = Path.Combine(wwwRootPath, "Images", "ic_campus_logo.png");

            if (System.IO.File.Exists(logoPath))
            {
                var logo = iTextSharp.text.Image.GetInstance(logoPath);
                logo.ScaleAbsolute(80f, 80f); 
                logo.Alignment = Element.ALIGN_CENTER;
                doc.Add(logo);

                doc.Add(new Paragraph("\n"));
            }
            else
            {
                
                var warningFont = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 10, BaseColor.Gray);
                doc.Add(new Paragraph("[Logo missing: ic_campus_logo.png]", warningFont));
            }
        }

    }
}
