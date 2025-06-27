using Ninjasoft.AzureDevOpsClient.Models;
using Ninjasoft.AzureDevOpsClient.Repositories.Utilities;
using System.Globalization;

namespace Ninjasoft.AzureDevOpsClient.Repositories
{    
    public class ReleaseApiRepository
    {
        public ReleaseApiRepository(IAzureDevOpsUrlBuilderFactory factory)
        {
            _factory = factory;
        }

        public async Task<Release> GetReleaseAsync(int releaseId, Action<QueryStringBuilder>? queryStringFunc = null) =>
            await _factory.Create()
                .WithSubDomain("vsrm")
                .WithPath($"_apis/release/releases/{releaseId}")
                .WithQueryString(queryStringFunc)
                .Get()
                .DeserializeResponseAsync<Release>();


        public async Task<List<Release>> GetReleasesAsync(Action<QueryStringBuilder>? queryStringFunc = null) =>
            await _factory.Create()
                .WithSubDomain("vsrm")
                .WithPath($"_apis/release/releases")
                .WithQueryString(queryStringFunc)
                .Get()
                .DeserializeResponseListAsync<Release>();

        public async Task<List<Release>> GetReleasesBetweenAsync(DateTime start, DateTime end) =>
           await _factory.Create()
                .WithSubDomain("vsrm")
                .WithPath($"_apis/release/releases")
                .WithQueryString(t => 
                       t.Set("minCreatedTime", start.ToString("s", DateTimeFormatInfo.CurrentInfo))
                        .Set("maxCreatedTime", end.ToString("s", DateTimeFormatInfo.CurrentInfo)))
                .Get()
                .DeserializeResponseListAsync<Release>();

        public async Task<List<Release>> GetReleasesAfterAsync(DateTimeOffset createdAfterDateTime) =>
            await _factory.Create()
                .WithSubDomain("vsrm")
                .WithPath("_apis/release/releases")
                .WithQueryString(t => t.Set("minCreatedTime", createdAfterDateTime.ToString("yyyy-MM-ddThh:mm:ss.fffZ")))
                .Get()
                .DeserializeResponseListAsync<Release>();

        public async Task<List<ReleaseDefinition>> GetReleaseDefinitionsAsync(Action<QueryStringBuilder>? queryStringFunc = null) =>
            await _factory.Create()
                .WithSubDomain("vsrm")
                .WithPath("_apis/release/definitions")
                .WithQueryString(queryStringFunc)
                .Get()
                .DeserializeResponseListAsync<ReleaseDefinition>();

        public async Task<ReleaseDefinition> GetReleaseDefinitionAsync(int releaseDefinitionId) =>
            await _factory.Create()
                .WithSubDomain("vsrm")
                .WithPath($"_apis/release/definitions/{releaseDefinitionId}")
                .Get()
                .DeserializeResponseAsync<ReleaseDefinition>();

        public async Task<string> GetReleaseDefinitionStringAsync(int releaseDefinitionId) =>
            await _factory.Create()
                .WithSubDomain("vsrm")
                .WithPath($"_apis/release/definitions/{releaseDefinitionId}")
                .GetAsync();

        public async Task<List<Release>> GetReleasesForDefinitionAsync(int releaseDefinitionId, DateTimeOffset createdAfterDateTime) =>
            await _factory.Create()
                .WithSubDomain("vsrm")
                .WithPath("_apis/release/releases")
                .WithQueryString(t => 
                    t.Set("definitionId", releaseDefinitionId)
                    .Set("minCreatedTime", createdAfterDateTime.ToString("yyyy-MM-ddThh:mm:ss.fffZ")))
                .Get()
                .DeserializeResponseListAsync<Release>();

        private readonly IAzureDevOpsUrlBuilderFactory _factory;
    }
}
