namespace UNI_ASSETS.DTOs
{
    public class SubmissionDto
    {
        public string AssetId { get; set; }
        public string StaffId { get; set; }
        public string Condition { get; set; }
        public string ImageBase64 { get; set; }
        public string Date { get; set; } // can parse into DateTime if needed
        public string Note { get; set; }
        public string Location { get; set; }
    }
}
