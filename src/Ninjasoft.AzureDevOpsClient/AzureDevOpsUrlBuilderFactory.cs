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
        public string Version { get; set; } = "7.1";

        public AzureDevOpsUrlBuilder Create() =>
            new AzureDevOpsUrlBuilder(personalAccessToken, HttpUtility.UrlPathEncode(organization), HttpUtility.UrlPathEncode(project), Version);
    }
}