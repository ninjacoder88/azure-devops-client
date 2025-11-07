namespace Ninjasoft.AzureDevOpsClient
{
    public interface IAzureDevOpsRepositoryFactory
    {
        IAzureDevOpsRepository Create(string personalAccessToken, string organization, string projectName = "");
        IAzureDevOpsRepository Create(IAzureDevOpsUrlBuilderFactory azureDevOpsUrlBuilderFactory);
    }

    public class AzureDevOpsRepositoryFactory : IAzureDevOpsRepositoryFactory
    {
        public IAzureDevOpsRepository Create(string personalAccessToken, string organization, string projectName = "") =>
            new AzureDevOpsRepository(personalAccessToken, organization, projectName);

        public IAzureDevOpsRepository Create(IAzureDevOpsUrlBuilderFactory azureDevOpsUrlBuilderFactory) =>
            new AzureDevOpsRepository(azureDevOpsUrlBuilderFactory);
    }
}
