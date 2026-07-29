using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Ninjasoft.AzureDevOpsClient.Models;
using Ninjasoft.AzureDevOpsClient.Repositories.Utilities;

namespace Ninjasoft.AzureDevOpsClient
{
    public class AzureDevOpsUrlBuilderV2
    {
        public AzureDevOpsUrlBuilderV2(string organization, string? project, string apiVersion = "6.0")
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
            QueryStringBuilder builder = new QueryStringBuilder();
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
        private string _apiVersion;
        private string? _queryString;
        private string _organization;
        private string? _project;
    }

    internal class Http
    {
        public Http(string personalAccessToken)
        {
            _personalAccessToken = personalAccessToken;
        }

        public Task<string> Get(string url)
        {
            Func<HttpClient, Task<HttpResponseMessage>> func = async (client) => await client.GetAsync(url);
            Action<HttpResponseMessage> action = response =>
            {
                if (response.Headers.TryGetValues("x-ms-continuationtoken", out IEnumerable<string>? values))
                {
                    string? continutationToken = values.FirstOrDefault();
                    if (!string.IsNullOrEmpty(continutationToken))
                        _continuationToken = continutationToken;
                }
            };

            return HttpInternalAsync(url, func, action);
        }

        public async Task<string> Post(string url, string json) => 
            await HttpInternalAsync(url, async (client) =>
                await client.PatchAsync(url, new StringContent(json, Encoding.UTF8, "application/json-patch+json")));

        public async Task<string> PatchAsync(string url, string json) => await HttpInternalAsync(url, async (client) =>
            await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json")));

        private async Task<string> HttpInternalAsync(string url, Func<HttpClient, Task<HttpResponseMessage>> func, Action<HttpResponseMessage>? postResponseAction = null)
        {
            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{string.Empty}:{_personalAccessToken}")));

                HttpResponseMessage response = await func(client);
                string? responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"{url}\r\n{response.StatusCode} - {responseContent}");

                if (postResponseAction != null)
                    postResponseAction(response);

                //_responseContent = responseContent;
                return responseContent;
            }
        }

        private readonly string _personalAccessToken;
        private string _continuationToken;
    }

    public static class AzureDevOpsUrlBuilderV2Extensions
    {
        public static async Task<string> Get(this AzureDevOpsUrlBuilderV2 builder, string personalAccessToken)
        {
            for(int i = 0; i < 5; i++)
            {
                await Task.Delay(i * 1000);
                try
                {
                    return await new Http(personalAccessToken).Get(builder.Build());
                }
                catch
                {
                    if (i == 4)
                        throw;
                }
            }
            return string.Empty;
        }

        public static async Task<string> PostAsync(this AzureDevOpsUrlBuilderV2 builder, string personalAccessToken, object input)
        {
            return await new Http(personalAccessToken).Post(builder.Build(), JsonConvert.SerializeObject(input));
        }

        public static async Task<string> PatchAsync(this AzureDevOpsUrlBuilderV2 builder, string personalAccessToken, object input)
        {
            return await new Http(personalAccessToken).PatchAsync(builder.Build(), JsonConvert.SerializeObject(input));
        }
    }
}
