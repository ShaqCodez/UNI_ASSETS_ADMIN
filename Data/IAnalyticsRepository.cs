using System.Timers;
using UNI_ASSETS.Models;

namespace UNI_ASSETS.Data
{
    public interface IAnalyticsRepository:IBaseRepository<AppAnalytics>
    {
        void Start();
        void ChangeInterval(int interval);
        void Stop();
       TimeSpan TimeLeft { get; }
        int RefreshRate { get;  }
        
    }
    public class AnalyticsRepository : BaseRepository<AppAnalytics>, IAnalyticsRepository
    {
        private int iRefreshRate;
        private System.Timers.Timer timer;
      
        private DateTime StartTime,EndTime;
        public AnalyticsRepository(AppDbContext context) : base(context)
        {
            timer = new System.Timers.Timer();
            timer.Enabled = true;
            timer.Elapsed += Timer_Elapsed;
            
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var submissions = context.Submissions;
            if (submissions != null)
            {
                if(submissions.Count()> 0)
                {
                    AppAnalytics analytics = new AppAnalytics();
                    analytics.Submissions = submissions.ToList();
                    
                    context.Analytics.Add(analytics);
                    int Index = context.Analytics.Count() - 1;
                    foreach (var submission in context.Submissions)
                    {
                        if(submission.LogId <1)
                        submission.LogId = context.Analytics.ToList()[Index].LogId;
                    }
                }
            }
        }

        public int RefreshRate => iRefreshRate;

        public TimeSpan TimeLeft => EndTime - DateTime.Now;

        public void ChangeInterval(int interval)
        {
            iRefreshRate = interval;
        }

        public void Start()
        {
            timer.Interval = iRefreshRate;
            StartTime = DateTime.Now;
            EndTime = StartTime.Add(new TimeSpan(iRefreshRate));
            timer.Start();
            timer.AutoReset = true;
           
            
        }


        public void Stop()
        {
            timer.Stop();
        }
    }
}
