namespace UNI_ASSETS.Data
{
    public interface IRepositoryWrapper
    {
       ISubmissionRepository SubmissionRepository { get; }
        IAssetRepository AssetRepository { get; }
        IStaffRepository StaffRepository { get; }
        IAnalyticsRepository AnalyticsRepository { get; }
        void Save();
    }
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly IdentityContext context;
            private ISubmissionRepository _submissionRepository;
        private IAssetRepository _assetRepository;
        private readonly IAnalyticsRepository analyticsRepository;
        private readonly IStaffRepository staffRepository;
        public RepositoryWrapper(IdentityContext dbContext)
        {
            this.context = dbContext;
            analyticsRepository = new AnalyticsRepository(context);
        }
        public ISubmissionRepository SubmissionRepository => _submissionRepository??new SubmissionRepository(context);

        public IAssetRepository AssetRepository => _assetRepository??new AssetRepository(context);

        public IAnalyticsRepository AnalyticsRepository => analyticsRepository??new AnalyticsRepository(context);

        public IStaffRepository StaffRepository => staffRepository??new StaffRepository(context);

        public void Save() => context.SaveChanges();
    }
}
