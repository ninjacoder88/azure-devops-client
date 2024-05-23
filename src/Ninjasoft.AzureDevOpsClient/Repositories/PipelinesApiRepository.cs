using Ninjasoft.AzureDevOpsClient.Models;

namespace Ninjasoft.AzureDevOpsClient.Repositories
{
    public class PipelinesApiRepository
    {
        public PipelinesApiRepository(IAzureDevOpsUrlBuilderFactory factory)
        {
            _factory = factory;
        }

        public async Task<List<Pipeline>> GetPipelinesAsync() =>
            await _factory.Create()
                            .WithPath("_apis/pipelines")
                            .Get()
                            .DeserializeResponseListAsync<Pipeline>();

        private readonly IAzureDevOpsUrlBuilderFactory _factory;
    }
}
