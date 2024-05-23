using Ninjasoft.AzureDevOpsClient.Models;

namespace Ninjasoft.AzureDevOpsClient.Repositories
{
    public class ProjectsApiRepository
    {
        public ProjectsApiRepository(IAzureDevOpsUrlBuilderFactory factory)
        {
            _factory = factory;
        }

        public async Task<List<TeamProjectReference>> GetProjectsForOrganizationAsync() =>
            await _factory.Create()
                            .WithPath("_apis/projects")
                            .Get()
                            .DeserializeResponseListAsync<TeamProjectReference>();

        private readonly IAzureDevOpsUrlBuilderFactory _factory;
    }
}
