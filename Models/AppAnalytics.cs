using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using System.Timers;
using UNI_ASSETS.Data;

namespace UNI_ASSETS.Models
{
    /// <summary>
    /// Represents all the analytics perfomed on submission data
    /// </summary>
    
    public class AppAnalytics 
    {
        
        /// <summary>
        /// Primary Key- Represents the state of the Asset Management Team's stat-line at a particular time
        /// </summary>
        public int LogId { get; set; }
        /// <summary>
        /// Measures the efficiency of the asset management team in processing reports.(Decreasing trend suggests improved workflow)
        /// </summary>
        public TimeSpan AverageDuration { get => GetAverageDuration(); }
        /// <summary>
        /// reference to all seubmission made
        /// </summary>
        public List<Submission> Submissions { get; set; }
        /// <summary>
        /// Gets the number of submissions that went unreviewed for a week
        /// </summary>
        public int ReviewBackLogWeekly { get => GetReviewBackLog7days(); }
        /// <summary>
        /// Gets the number of submissions that went unreviewed for more than a month
        /// </summary>
        public int ReviewBackLogMonthly { get => GetReviewBackLog30days(); }
        /// <summary>
        /// Tracks the pace at which the system is clearing submitted reports per week
        /// </summary>
        public int ReviewStatusVelocity { get => GetVelocity(); }
        /// <summary>
        /// Gets the percentage of Submissions that were reviewed
        /// </summary>
        public double PercentageReviewed { get => GetForStatus(ReviewStatus.Reviewed); }
        /// <summary>
        /// Gets the percentage of Submissions that are still pending review
        /// </summary>
        public double PercentagePending { get => GetForStatus(ReviewStatus.Pending); }
        /// <summary>
        /// Gets the percentage of Submissions that were found to be functional
        /// </summary>
        public double PercentageRejected { get => GetForStatus(ReviewStatus.Rejected); }
        private double GetForStatus(ReviewStatus reviewStatus)
        {
            int totalSubmissions = Submissions.Count();
            int totalForStatus = Submissions.Count(x => x.ReviewStatus == reviewStatus);
            return totalSubmissions / totalForStatus;
        }
        /// <summary>
        /// Gets the number of Reports for a particular Asset
        /// </summary>
        /// <param name="AssetId">The identifier for the required asset</param>
        /// <returns>The Number of submissions made for an asset</returns>
        public int ReportsPerAsset(string AssetId)
        {
            return Submissions.Count(x => x.AssetId == AssetId);
        }
        /// <summary>
        /// Gets the Repots on the particular asset
        /// </summary>
        /// <param name="AssetId">The identifier for the required asset</param>
        /// <returns>The List of submissions on the particular asset</returns>
        public List<Submission> ReportsForAsset(string AssetId)
        {
            return Submissions.Where(x => x.AssetId == AssetId).ToList();
        }
        private TimeSpan GetAverageDuration()
        {
            var duration = Submissions.Select(x => x.DateReviewed - x.CreatedDate);
            TimeSpan AverageDuration = new TimeSpan();
            foreach (var individualspan in duration)
            {
                AverageDuration.Add(individualspan);
            }
            AverageDuration.Divide(duration.Count());
            return AverageDuration;
        }
        /// <summary>
        /// Gets the number of submissions by an employee per x number of days
        /// </summary>
        /// <param name="StaffId">The unique identifier for an employee</param>
        /// <param name="TimeInDays">The amount of time in days that the submission search is over</param>
        /// <returns>The Volume by an employee per x number of days</returns>
        public int GetSubmissionVolume(string StaffId,int TimeInDays)
        {
            int Volume = 0;
            Volume = Submissions.Count(x => x.StaffId == StaffId && (DateTime.Now - x.CreatedDate).Days <= TimeInDays);
            return Volume;
        }
        /// <summary>
        /// Count of reports originating from specific area
        /// </summary>
        /// <param name="Location">The location where reports are originating(case-insensitive) </param>
        /// <returns></returns>
        public int GetLocationSubissionDensity(string Location)
        {
            return Submissions.Count(x=>x.Location.Equals(Location,StringComparison.CurrentCultureIgnoreCase));    
        }
        private int GetReviewBackLog7days()
        {
            
           int total = Submissions.Count(x=>(DateTime.Now-x.CreatedDate).Days <= 7 && x.ReviewStatus == ReviewStatus.Pending);

            return total;
        }
        private int GetReviewBackLog30days()
        {
            int total = Submissions.Count(x => (DateTime.Now - x.CreatedDate).Days >=30 && x.ReviewStatus == ReviewStatus.Pending);

            return total;
        }
        private int GetVelocity()
        {
            int count = 0;
            var reviewedSubmissions = Submissions.Where(x => x.ReviewStatus == ReviewStatus.Reviewed).ToList();
            count = reviewedSubmissions.Count(x =>
            {
                int days = (x.DateReviewed - DateTime.Now).Days;
                return days > 0 && days <= 7 ;
            });
            return count;
        }
        /// <summary>
        /// Gets the number of reports originating from an area based on the location and review status
        /// </summary>
        /// <param name="Location">The Location to perform the search on</param>
        /// <param name="reviewStatus">The Review Status of the Asset</param>
        /// <returns></returns>
        public int FilterLocationStatus(string Location,ReviewStatus reviewStatus)
        {
            return Submissions.Count(x => x.Location.Equals(Location, StringComparison.CurrentCultureIgnoreCase) && x.ReviewStatus == reviewStatus);
        }
    }
}
