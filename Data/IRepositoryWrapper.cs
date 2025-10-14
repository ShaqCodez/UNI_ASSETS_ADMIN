namespace UNI_ASSETS.Data
{
    public interface IRepositoryWrapper
    {
       ISubmissionRepository SubmissionRepository { get; }
        IAssetRepository AssetRepository { get; }
        void Save();
    }
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly AppDbContext context;
            private ISubmissionRepository _submissionRepository;
        private IAssetRepository _assetRepository;
        public RepositoryWrapper(AppDbContext dbContext)
        {
            this.context = dbContext;

        }
        public ISubmissionRepository SubmissionRepository => _submissionRepository??new SubmissionRepository(context);

        public IAssetRepository AssetRepository => _assetRepository??new AssetRepository(context);
        public void Save() => context.SaveChanges();
    }
}
