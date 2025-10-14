using System.Globalization;

namespace UNI_ASSETS.Models
{
    public class Submission
    {
        public int SubmissionId { get; set; }
        public string AssetId { get; set; }
        public string StaffId { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte[] Image { get; set; }
        public string Note { get; set; }
        public string Location { get; set; }
        public Asset Asset { get; set; }
        public AppUser Staff { get; set; }
        public bool Reviewed { get; set; }
        public string ReviewStatus { get; set; }
    }
}
