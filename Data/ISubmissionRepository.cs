using Microsoft.EntityFrameworkCore;
using UNI_ASSETS.Models;

namespace UNI_ASSETS.Data
{
    public interface ISubmissionRepository:IBaseRepository<Submission>
    {
        Asset GetLikelyToReplace();
        IEnumerable<Submission> GetAllAssetSubmissions(string AssetId);
    }
    public class SubmissionRepository : BaseRepository<Submission>, ISubmissionRepository
    {
        public SubmissionRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Submission> GetAllAssetSubmissions(string AssetId)
        {
            return context.Submissions.Where(sub=>sub.Equals(AssetId)).Include(x=>x.Asset);
        }

        public Asset GetLikelyToReplace()
        {
            Asset asset = null;
            List<KeyValuePair<Asset, int>> assetpair = new List<KeyValuePair<Asset, int>>();
            List<Submission> submissions = new(context.Submissions); 
            for (int i = 0; i < submissions.Count(); i++)
            {
                assetpair.Add(new KeyValuePair<Asset, int>(submissions[i].Asset, submissions.Count(x=>x.AssetId == submissions[i].AssetId)));
                
            }
            asset = assetpair.OrderBy(x=>x.Value).First().Key;
            return asset; 
        }
        
    }
}
