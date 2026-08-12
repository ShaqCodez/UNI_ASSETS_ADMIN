using UNI_ASSETS.Models;

namespace UNI_ASSETS.Data
{
    public interface IAssetRepository:IBaseRepository<Asset>
    {

    }
    public class AssetRepository : BaseRepository<Asset>, IAssetRepository
    {
        public AssetRepository(IdentityContext context) : base(context)
        {
        }

    }
}
