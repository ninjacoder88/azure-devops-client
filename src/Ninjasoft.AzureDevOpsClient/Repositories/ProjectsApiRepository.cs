using Ninjasoft.AzureDevOpsClient.Models;

namespace Ninjasoft.AzureDevOpsClient.Repositories
{
    public class ProjectsApiRepository(IAzureDevOpsUrlBuilderFactory _factory)
    {
        public async Task<List<TeamProjectReference>> GetProjectsForOrganizationAsync() =>
            await _factory.Create()
                            .WithPath("_apis/projects", includeProject: false)
                            .Get()
                            .DeserializeResponseListAsync<TeamProjectReference>();
    }
}
