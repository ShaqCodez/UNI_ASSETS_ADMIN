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
        public DateTime DateReviewed { get; private set; }
        private bool IsReviewed;
        public byte[] Image { get; set; }
        public string Note { get; set; }
        public string Location { get; set; }
        public Asset Asset { get; set; }
        public string Condition { get; set; }
        public AppUser Staff { get; set; }
        public bool Reviewed { get => IsReviewed; set 
            {
                DateReviewed = DateTime.Now;
                ReviewStatus = ReviewStatus.Reviewed;
                IsReviewed = value;
            } 
        }
        public ReviewStatus ReviewStatus { get; set; }
        public AppAnalytics Analysis { get; set; }
    }
}
