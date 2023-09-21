namespace Ninjasoft.AzureDevOpsClient
{
    public interface IAzureDevOpsUrlBuilderFactory
    {
        AzureDevOpsUrlBuilder Create();
    }

    public class AzureDevOpsUrlBuilderFactory : IAzureDevOpsUrlBuilderFactory
    {
        public AzureDevOpsUrlBuilderFactory(string personalAccessToken, string organization, string project)
        {
            _personalAccessToken = personalAccessToken;
            _organization = organization;
            _project = project;
        }

        public AzureDevOpsUrlBuilder Create()
        {
            return new AzureDevOpsUrlBuilder(_personalAccessToken, _organization, _project);
        }

        private readonly string _organization;
        private readonly string _personalAccessToken;
        private readonly string _project;
    }
}