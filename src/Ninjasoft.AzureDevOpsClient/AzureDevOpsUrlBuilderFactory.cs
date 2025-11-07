using System.Web;

namespace Ninjasoft.AzureDevOpsClient
{
    public interface IAzureDevOpsUrlBuilderFactory
    {
        AzureDevOpsUrlBuilder Create();
    }

    public class AzureDevOpsUrlBuilderFactory(string personalAccessToken, string organization, string project = "") 
        : IAzureDevOpsUrlBuilderFactory
    {
        public AzureDevOpsUrlBuilder Create() =>
            new AzureDevOpsUrlBuilder(personalAccessToken, organization, HttpUtility.UrlPathEncode(project));
    }
}