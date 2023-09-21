using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Ninjasoft.AzureDevOpsClient
{
    public class AzureDevOpsUrlBuilder
    {
        public AzureDevOpsUrlBuilder(string personalAccessToken, string organization, string project)
        {
            _personalAccessToken = personalAccessToken;
            _organization = organization;
            _project = project;
        }

        public async Task<T> DeserializeResponseAsync<T>()
        {
            await _task;
            return JsonConvert.DeserializeObject<T>(_responseContent);
        }

        public AzureDevOpsUrlBuilder Get()
        {
            _task = GetWithRetryAsync();
            return this;
        }

        public async Task<string> GetAsync()
        {
            return await GetWithRetryAsync();
        }

        public AzureDevOpsUrlBuilder Patch(string json)
        {
            _task = PatchInternalAsync(json);
            return this;
        }

        public async Task<string> PatchAsync(string json)
        {
            return await PatchInternalAsync(json);
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

        public AzureDevOpsUrlBuilder WithSubDomain(string subDomain)
        {
            _subDomain = $"{subDomain}.";
            return this;
        }

        private async Task<string> GetInternalAsync()
        {
            string queryString = !string.IsNullOrEmpty(_queryString) ? $"&{_queryString}" : "";
            var url = $"https://{_subDomain}dev.azure.com/{_path}?api-version={_apiVersion}{queryString}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var authString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{string.Empty}:{_personalAccessToken}"));
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", authString);

                var response = await client.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"{url}\r\n{responseContent}");

                _responseContent = responseContent;
                return responseContent;
            }
        }

        private async Task<string> PatchInternalAsync(string json)
        {
            string queryString = !string.IsNullOrEmpty(_queryString) ? $"&{_queryString}" : "";
            var url = $"https://{_subDomain}dev.azure.com/{_path}?api-version={_apiVersion}{queryString}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{string.Empty}:{_personalAccessToken}")));

                var response = await client.PatchAsync(url, new StringContent(json, Encoding.UTF8, "application/json-patch+json"));
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"{url}\r\n{responseContent}");

                _responseContent = responseContent;
                return responseContent;
            }
        }

        private async Task<string> GetWithRetryAsync()
        {
            int attempt = 0;
            while (true)
            {
                await Task.Delay(attempt * 3000);

                try
                {
                    attempt++;
                    return await GetInternalAsync();
                }
                catch
                {
                    if (attempt == 5)
                        throw;
                }
            }
        }

        private string _apiVersion = "6.0";
        private string _path;
        private readonly string _personalAccessToken;
        private readonly string _organization;
        private readonly string _project;
        private string _queryString;
        private string _responseContent;
        private string _subDomain = "";
        private Task<string> _task;
    }
}
