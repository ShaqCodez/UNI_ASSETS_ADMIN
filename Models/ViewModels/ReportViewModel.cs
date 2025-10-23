namespace UNI_ASSETS.Models.ViewModels
{
    public class ReportViewModel
    {
        public int TotalAssets { get; set; }
        public int TotalSubmissions { get; set; }
        public double PercentageReviewed { get; set; }
        public double PercentagePending { get; set; }
        public double PercentageRejected { get; set; }
        public TimeSpan AverageDuration { get; set; }
        public int ReviewBackLogWeekly { get; set; }
        public int ReviewBackLogMonthly { get; set; }
        public int ReviewStatusVelocity { get; set; }
    }
    public class AssetReportViewModel
    {
        public Asset Asset { get; set; }
        public int TotalReports { get; set; }
        public double PercentageReviewed { get; set; }
        public double PercentagePending { get; set; }
        public double PercentageRejected { get; set; }
        public TimeSpan AverageDuration { get; set; }
        public List<Submission> Reports { get; set; }
    }
}
