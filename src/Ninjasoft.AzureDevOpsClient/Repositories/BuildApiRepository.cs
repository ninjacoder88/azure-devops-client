using Ninjasoft.AzureDevOpsClient.Models;

namespace Ninjasoft.AzureDevOpsClient.Repositories
{
    public class BuildApiRepository(IAzureDevOpsUrlBuilderFactory _factory)
    {
        public async Task<Build> GetBuildAsync(int buildId) =>
            await _factory.Create()
                            .WithPath($"_apis/build/builds/{buildId}")
                            .Get()
                            .DeserializeResponseAsync<Build>();

        public async Task<List<Change>> GetBuildChangesAsync(int buildId) =>
            await _factory.Create()
                            .WithPath($"_apis/build/builds/{buildId}/changes")
                            .Get()
                            .DeserializeResponseListAsync<Change>();

        public async Task<List<ResourceRef>> GetBuildWorkItemResourcesAsync(int buildId) =>
            await _factory.Create()
                            .WithPath($"_apis/build/builds/{buildId}/workItems")
                            .Get()
                            .DeserializeResponseListAsync<ResourceRef>();
    }
}
