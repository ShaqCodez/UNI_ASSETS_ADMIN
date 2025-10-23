using Microsoft.Identity.Client;

namespace UNI_ASSETS.Models.ViewModels
{
    public class DetailsViewModel
    {
        public int ReportCount { get; private set; }
        public DateTime LastIncident { get; set; }
       
        public List<string> Venues { get; set; }
        public Asset Asset { get; set; }
        private readonly List<Submission> submissions;

        public DetailsViewModel(Asset asset, List<Submission> submission)
        {
            this.Asset = asset;
            this.submissions = submission;
            SetCount();
        }
        void SetCount()
        {
            ReportCount = submissions.Count;
        }
        
    }
}
/*
 Create,delete and update User(backend done)
Asset Reviewing,Analytics and reports
 Login Page
 */