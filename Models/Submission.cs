using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace UNI_ASSETS.Models
{
    public enum ReviewStatus
    {
        Pending,Reviewed,Rejected
    }
    public class Submission
    {
       
        public int SubmissionId { get; set; }
      public int? LogId { get; set; }
        public string AssetId { get; set; }
        public string StaffId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DateReviewed { get;  set; }
       
        public byte[] Image { get; set; }
        public string Note { get; set; }
        public string Location { get; set; }
       
        public string Condition { get; set; }
       
        public ReviewStatus ReviewStatus { get; set; } = ReviewStatus.Pending;

        [ForeignKey(nameof(AssetId))]
        public Asset Asset { get; set; }

        [ForeignKey(nameof(StaffId))]
        public AppUser Staff { get; set; }

        [ForeignKey(nameof(LogId))]
        public AppAnalytics Analysis { get; set; }

        public void MarkReviewed()
        {
            ReviewStatus = ReviewStatus.Reviewed;
            DateReviewed = DateTime.Now;
        }
       
    }
}
