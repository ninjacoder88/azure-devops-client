using Ninjasoft.AzureDevOpsClient.Models;

namespace Ninjasoft.AzureDevOpsClient.Repositories
{
    public class WorkApiRepository(IAzureDevOpsUrlBuilderFactory _factory)
    {
        public async Task<List<TeamSettingsIteration>> GetIterationsAsync(string backlog) =>
            await _factory.Create()
                            .WithPath($"{Uri.EscapeDataString(backlog)}/_apis/work/teamsettings/iterations")
                            .Get()
                            .DeserializeResponseListAsync<TeamSettingsIteration>();

        public async Task<IterationWorkItems> GetIterationWorkItemsAsync(string backlog, string iterationId) =>
            await _factory.Create()
                            .WithPath($"{Uri.EscapeDataString(backlog)}/_apis/work/teamsettings/iterations/{iterationId}/workitems")
                            .Get()
                            .DeserializeResponseAsync<IterationWorkItems>();

        public async Task<BacklogWorkItems> GetBacklogWorkItemsAsync(string backlog, string category) =>
            await _factory.Create()
                            .WithPath($"{backlog}/_apis/work/backlogs/{category}/workitems")
                            .Get()
                            .DeserializeResponseAsync<BacklogWorkItems>();

    }
}
