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

        public async Task<List<Release>> GetReleasesAsync(DateTime? start = null, DateTime? end = null)
        {
            Action<QueryStringBuilder>? queryStringFunc = q =>
            {
                if (start != null)
                    q.Set("minCreatedTime", start.Value.ToString("s", DateTimeFormatInfo.CurrentInfo));
                if (end != null)
                    q.Set("maxCreatedTime", end.Value.ToString("s", DateTimeFormatInfo.CurrentInfo));
            };

            return await _factory.Create()
                .WithSubDomain("vsrm")
                .WithPath($"_apis/release/releases")
                .WithQueryString(queryStringFunc)
                .Get()
                .DeserializeResponseListAsync<Release>();
        }
           
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

        public async Task<List<Release>> GetReleasesForDefinitionAsync(int releaseDefinitionId, DateTimeOffset? createdAfterDateTime = null,
            DateTimeOffset? createdBeforeDateTime = null)
        {
            Action<QueryStringBuilder>? queryStringFunc = (q) =>
            {
                q.Set("definitionId", releaseDefinitionId);
                if (createdAfterDateTime != null)
                    q.Set("minCreatedTime", createdAfterDateTime.Value.ToString("yyyy-MM-ddThh:mm:ss.fffZ"));
                if (createdBeforeDateTime != null)
                    q.Set("maxCreatedTime", createdBeforeDateTime.Value.ToString("yyyy-MM-ddThh:mm:ss.fffZ"));
            };

            return await _factory.Create()
                .WithSubDomain("vsrm")
                .WithPath("_apis/release/releases")
                .WithQueryString(queryStringFunc)
                .Get()
                .DeserializeResponseListAsync<Release>();
        }
            

        private readonly IAzureDevOpsUrlBuilderFactory _factory;
    }
}
