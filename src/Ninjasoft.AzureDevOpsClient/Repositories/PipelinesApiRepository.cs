using Ninjasoft.AzureDevOpsClient.Models;

namespace Ninjasoft.AzureDevOpsClient.Repositories
{
    public class PipelinesApiRepository(IAzureDevOpsUrlBuilderFactory _factory)
    {
        public async Task<List<Pipeline>> GetPipelinesAsync() =>
            await _factory.Create()
                            .WithPath("_apis/pipelines")
                            .Get()
                            .DeserializeResponseListAsync<Pipeline>();
    }
}
