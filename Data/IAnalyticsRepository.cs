using System.Timers;
using UNI_ASSETS.Models;

namespace UNI_ASSETS.Data
{
    public interface IAnalyticsRepository:IBaseRepository<AppAnalytics>
    {
       
        
    }
    public class AnalyticsRepository : BaseRepository<AppAnalytics>, IAnalyticsRepository
    {
        public AnalyticsRepository(AppDbContext context) : base(context)
        {
            
        }

     
    }
}
