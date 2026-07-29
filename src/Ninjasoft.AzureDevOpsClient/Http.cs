using System.Net.Http.Headers;
using System.Text;

namespace Ninjasoft.AzureDevOpsClient
{
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
}