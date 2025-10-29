using System.Linq.Expressions;
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
    public interface IStaffRepository : IBaseRepository<AppUser>
    {
        AppUser GetByCondition(Expression<Func<AppUser, bool>> where);
    }
    public class StaffRepository : BaseRepository<AppUser>, IStaffRepository
    {
        public StaffRepository(AppDbContext context):base(context)
        {
                
        }

        public AppUser GetByCondition(Expression<Func<AppUser, bool>> where)
        {
            return context.Staff.FirstOrDefault(where);
        }
    }
}
