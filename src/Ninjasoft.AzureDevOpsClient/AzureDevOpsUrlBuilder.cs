using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Ninjasoft.AzureDevOpsClient.Models;
using Ninjasoft.AzureDevOpsClient.Repositories.Utilities;

namespace Ninjasoft.AzureDevOpsClient
{
    public class AzureDevOpsUrlBuilder
    {
        public AzureDevOpsUrlBuilder(string personalAccessToken, string organization, string project, string apiVersion = "7.1")
        {
            _personalAccessToken = personalAccessToken;
            _organization = organization;
            _project = project;
            _apiVersion = apiVersion;
        }

        public async Task<T?> DeserializeResponseAsync<T>()
        {
            await _task;
            return JsonConvert.DeserializeObject<T>(_responseContent);
        }

        public async Task<List<T>> DeserializeResponseListAsync<T>()
        {
            await _task;
            ResponseList<T>? list = JsonConvert.DeserializeObject<ResponseList<T>>(_responseContent);
            return list?.Value ?? [];
        }

        public AzureDevOpsUrlBuilder Get()
        {
            _task = GetWithRetryAsync();
            return this;
        }

        public async Task<string> GetAsync() => await GetWithRetryAsync();

        public AzureDevOpsUrlBuilder Patch(string json)
        {
            _task = PatchInternalAsync(json);
            return this;
        }

        public async Task<string> PatchAsync(string json)
        {
            return await PatchInternalAsync(json);
        }

        public AzureDevOpsUrlBuilder Post(string json)
        {
            _task = PostInternalAsync(json);
            return this;
        }

        public async Task<string> PostAsync(string json)
        {
            return await PostInternalAsync(json);
        }

        public AzureDevOpsUrlBuilder UsingApiVersion(string version)
        {
            _apiVersion = version;
            return this;
        }

        public AzureDevOpsUrlBuilder WithPath(string path, bool includeOrg = true, bool includeProject = true)
        {
            if (includeProject && includeOrg)
            {
                _path = $"{_organization}/{_project}/{path}/";
                return this;
            }

            if (includeProject)
            {
                _path = $"{_project}/{path}/";
                return this;
            }

            if (includeOrg)
            {
                _path = $"{_organization}/{path}/";
                return this;
            }

            _path = path;
            return this;
        }

        public AzureDevOpsUrlBuilder WithQueryString(string queryString)
        {
            _queryString = queryString;
            return this;
        }

        public AzureDevOpsUrlBuilder WithQueryString(Action<QueryStringBuilder>? queryStringFunc)
        {
            if (queryStringFunc != null)
            {
                QueryStringBuilder builder = new QueryStringBuilder();
                queryStringFunc(builder);
                string queryString = builder.Build();
                if (!string.IsNullOrEmpty(queryString))
                    _queryString = queryString;
            }
            return this;
        }

        public AzureDevOpsUrlBuilder WithSubDomain(string subDomain)
        {
            _subDomain = $"{subDomain}.";
            return this;
        }

        private async Task<string> HttpInternalAsync(Func<HttpClient, string, Task<HttpResponseMessage>> func, Action<HttpResponseMessage>? postResponseAction = null)
        {
            string queryString = !string.IsNullOrEmpty(_queryString) ? $"&{_queryString}" : "";
            string url = $"https://{_subDomain}dev.azure.com/{_path}?api-version={_apiVersion}{queryString}";

            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{string.Empty}:{_personalAccessToken}")));

                HttpResponseMessage response = await func(client, url);
                string? responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"{url}\r\n{response.StatusCode} - {responseContent}");

                if(postResponseAction != null)
                    postResponseAction(response);

                _responseContent = responseContent;
                return responseContent;
            }
        }

        private async Task<string> GetInternalAsync()
        {
            Func<HttpClient, string, Task<HttpResponseMessage>> func = async (client, url) => await client.GetAsync(url);
            Action<HttpResponseMessage> action = response =>
            {
                if (response.Headers.TryGetValues("x-ms-continuationtoken", out IEnumerable<string>? values))
                {
                    string? continutationToken = values.FirstOrDefault();
                    if (!string.IsNullOrEmpty(continutationToken))
                        _continuationToken = continutationToken;
                    else
                        _continuationToken = null;
                }
            };

            return await HttpInternalAsync(func, action);
        }

        private async Task<string> PatchInternalAsync(string json) => await HttpInternalAsync(async (client, url) => 
            await client.PatchAsync(url, new StringContent(json, Encoding.UTF8, "application/json-patch+json")));

        private async Task<string> PostInternalAsync(string json) =>
            await HttpInternalAsync(async (client, url) => 
            await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json")));

        private async Task<string> GetWithRetryAsync()
        {
            for(int i = 0; i < 5; i++)
            {
                await Task.Delay(i * 250);

                try
                {
                    return await GetInternalAsync();
                }
                catch
                {
                    if (i == 5)
                        throw;
                }
            }
            return null;
        }

        private string _apiVersion;
        private string _path;
        private readonly string _personalAccessToken;
        private readonly string _organization;
        private readonly string _project;
        private string _queryString;
        private string _responseContent;
        private string _subDomain = "";
        private Task<string> _task;
        private string? _continuationToken;
    }
}
