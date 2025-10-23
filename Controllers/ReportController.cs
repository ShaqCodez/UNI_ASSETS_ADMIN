using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UNI_ASSETS.Data;
using UNI_ASSETS.Models;
using UNI_ASSETS.Models.ViewModels;
using iTextSharp.LGPLv2;
using iTextSharp.text;
using iTextSharp.text.pdf;
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
        public IActionResult Reports()
        {
            var submissions = repository.SubmissionRepository.GetAll();
            return View(submissions);
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
        [HttpGet]
        public  IActionResult DownloadReport()
        {
            var submissions = repository.SubmissionRepository.GetSubmissionsWithDetails();

            using var stream = new MemoryStream();
            var doc = new Document(PageSize.A4, 25, 25, 25, 25);
            PdfWriter.GetInstance(doc, stream);
            doc.Open();

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

            return File(stream.ToArray(), "application/pdf", "CampusAssetReport.pdf");
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
    }
}
