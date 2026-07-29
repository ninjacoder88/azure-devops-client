using System.Text;
using Ninjasoft.AzureDevOpsClient.Repositories.Utilities;

namespace Ninjasoft.AzureDevOpsClient
{
    public class AzureDevOpsUrlBuilderV2
    {
        public AzureDevOpsUrlBuilderV2(string organization, string? project, string apiVersion = "7.1")
        {
            _apiVersion = apiVersion;
            _organization = organization;
            _project = project;
        }

        public AzureDevOpsUrlBuilderV2 SetSubDomain(string subDomain)
        {
            _subDomain = subDomain;
            return this;
        }

        public AzureDevOpsUrlBuilderV2 SetPath(string path)
        {
            _path = path;
            return this;
        }

        public AzureDevOpsUrlBuilderV2 SetQueryString(string queryString)
        {
            _queryString = queryString;
            return this;
        }

        public AzureDevOpsUrlBuilderV2 SetQueryString(Action<QueryStringBuilder> action)
        {
            QueryStringBuilder builder = new();
            action(builder);
            _queryString = builder.Build();
            return this;
        }

        public string Build()
        {
            StringBuilder sb = new ("https://");
            if (!string.IsNullOrWhiteSpace(_subDomain))
                sb.Append(_subDomain + ".");
            sb.Append($"dev.azure.com/{_organization}");
            if(!string.IsNullOrWhiteSpace(_project))
                sb.Append(_project + "/");
            if(!string.IsNullOrWhiteSpace(_path))
                sb.Append(_path);
            sb.Append("?api-version=" + _apiVersion);
            if(!string.IsNullOrWhiteSpace(_queryString))
                sb.Append("&" + _queryString);
            return sb.ToString();
        }

        private string? _subDomain;
        private string? _path;
        private readonly string _apiVersion;
        private string? _queryString;
        private readonly string _organization;
        private readonly string? _project;
    }
}
