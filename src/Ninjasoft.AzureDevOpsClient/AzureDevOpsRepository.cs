using MoreLinq;
using Newtonsoft.Json;
using Ninjasoft.AzureDevOpsClient.Models;
using System.Globalization;

namespace Ninjasoft.AzureDevOpsClient
{
    public class AzureDevOpsRepository
    {
        public AzureDevOpsRepository(IAzureDevOpsUrlBuilderFactory factory)
        {
            _factory = factory;
        }

        public async Task<List<GitPullRequest>> GetActivePullRequestsForRepositoryAsync(string repositoryId) => 
            await _factory.Create()
                            .WithPath($"_apis/git/repositories/{Uri.EscapeDataString(repositoryId)}/pullrequests")
                            .WithQueryString("searchCriteria.status=active")
                            .Get()
                            .DeserializeResponseListAsync<GitPullRequest>();
        
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

        public async Task<List<TestRunCoverage>> GetBuildCodeCoverageAsync(int buildId) =>
            await _factory.Create()
                            .WithPath("/_apis/test/codecoverage")
                            .WithQueryString($"buildId={buildId}&flags=1")
                            .Get()
                            .DeserializeResponseListAsync<TestRunCoverage>();

        public async Task<List<TeamSettingsIteration>> GetIterationsAsync(string backlog) =>
            await _factory.Create()
                            .WithPath($"{Uri.EscapeDataString(backlog)}/_apis/work/teamsettings/iterations")
                            .Get()
                            .DeserializeResponseListAsync<TeamSettingsIteration>();
        
        public async Task<IterationWorkItems> GetIterationWorkItemsAsync(string backlog, string iterationId) =>
            await _factory.Create()
                            .WithPath($"{Uri.EscapeDataString(backlog)}/_apis/work/teamsettings/iterations/{iterationId}/workitems")
                            .UsingApiVersion($"6.0-preview.1")
                            .Get()
                            .DeserializeResponseAsync<IterationWorkItems>();

        public async Task<List<Pipeline>> GetPipelinesAsync() =>
            await _factory.Create()
                            .WithPath("_apis/pipelines")
                            .Get()
                            .DeserializeResponseListAsync<Pipeline>();
        
        public async Task<List<TeamProjectReference>> GetProjectsForOrganizationAsync() =>
            await _factory.Create()
                            .WithPath("_apis/projects")
                            .Get()
                            .DeserializeResponseListAsync<TeamProjectReference>();
        
        public async Task<List<ResourceRef>> GetPullRequestWorkItemsAsync(string repositoryId, int pullRequestId) =>
            await _factory.Create()
                            .WithPath($"_apis/git/repositories/{repositoryId}/pullRequests/{pullRequestId}/workitems")
                            .Get()
                            .DeserializeResponseListAsync<ResourceRef>();

        public async Task<Release> GetReleaseAsync(int releaseId) =>
            await _factory.Create()
                            .WithSubDomain("vsrm")
                            .WithPath($"_apis/release/releases/{releaseId}")
                            .Get()
                            .DeserializeResponseAsync<Release>();
        
        public async Task<List<ReleaseDefinition>> GetReleaseDefinitionsAsync() =>
            await _factory.Create()
                            .WithSubDomain("vsrm")
                            .WithPath("_apis/release/definitions")
                            //.WithQueryString("$expand=Environments")
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

        public async Task<List<Release>> GetReleasesBetweenAsync(DateTime start, DateTime end) =>
            await _factory.Create()
                            .WithSubDomain("vsrm")
                            .WithPath($"_apis/release/releases")
                            .WithQueryString($"minCreatedTime={start.ToString("s", DateTimeFormatInfo.CurrentInfo)}&maxCreatedTime={end.ToString("s", DateTimeFormatInfo.CurrentInfo)}")
                            .Get()
                            .DeserializeResponseListAsync<Release>();

        public async Task<List<Release>> GetReleasesAfterAsync(DateTimeOffset createdAfterDateTime) =>
            await _factory.Create()
                            .WithSubDomain("vsrm")
                            .WithPath("_apis/release/releases")
                            .WithQueryString($"minCreatedTime={createdAfterDateTime.ToString("yyyy-MM-ddThh:mm:ss.fffZ")}")
                            .Get()
                            .DeserializeResponseListAsync<Release>();

        public async Task<List<GitRepository>> GetRepositoriesForProjectAsync() =>
            await _factory.Create()
                            .WithPath("/_apis/git/repositories")
                            .Get()
                            .DeserializeResponseListAsync<GitRepository>();
 
        public async Task<List<TestCaseResult>> GetTestCaseResultsAsync(int testRunId) =>
            await _factory.Create()
                            .WithPath($"_apis/test/runs/{testRunId}/results")
                            .Get()
                            .DeserializeResponseListAsync<TestCaseResult>();

        public async Task<List<TestRun>> GetTestRunsAsync(DateTime start, DateTime end, int buildId) =>       
            await _factory.Create()
                            .WithPath("_apis/test/runs")
                            .WithQueryString($"minLastUpdatedDate={start.ToString("s", DateTimeFormatInfo.CurrentInfo)}&maxLastUpdatedDate={end.ToString("s", DateTimeFormatInfo.CurrentInfo)}&buildIds={buildId}&$top=1")
                            .Get()
                            .DeserializeResponseListAsync<TestRun>();

        public async Task<List<TestRunCoverage>> GetTestRunCodeCoverageAsync(int testRunId) =>
            await _factory.Create()
                            .WithPath($"_apis/test/Runs/{testRunId}/codecoverage")
                            .WithQueryString("flags=1")
                            .Get()
                            .DeserializeResponseListAsync<TestRunCoverage>();

        public async Task<List<WorkItem>> GetWorkItemsAsync(List<int> workItemIds)
        {
            var batchedWorkItemIds = workItemIds.Batch(200);

            var list = new List<WorkItem>();
            foreach (var batchOfWorkItemIds in batchedWorkItemIds)
            {
                string workItemIdsStrings = string.Join(",", batchOfWorkItemIds);
                var response = await _factory.Create()
                                             .WithPath("_apis/wit/workitems")
                                             .WithQueryString($"ids={workItemIdsStrings}")
                                             .Get()
                                             .DeserializeResponseListAsync<WorkItem>();
                list.AddRange(response);
            }

            return list;
        }

        public async Task<List<WorkItemUpdate>> GetWorkItemUpdatesAsync(int workItemId) =>
            await _factory.Create()
                            .WithPath($"_apis/wit/workitems/{workItemId}/updates")
                            .Get()
                            .DeserializeResponseListAsync<WorkItemUpdate>();

        public async Task<WorkItem> GetWorkItemAsync(int workItemId, bool includeRelations = false)
        {
            string queryString = "";
            if (includeRelations)
                queryString += "$expand=Relations";

            return await _factory.Create()
                                .WithPath($"_apis/wit/workitems/{workItemId}")
                                .WithQueryString(queryString)
                                .Get()
                                .DeserializeResponseAsync<WorkItem>();
        }

        public async Task<GitPullRequestQuery<T>> PullRequestQueryAsync<T>(string repositoryId, GitPullRequestQueryInputList input) =>
            await _factory.Create()
                            .WithPath($"_apis/git/repositories/{repositoryId}/pullrequestquery")
                            .Post(JsonConvert.SerializeObject(input))
                            .DeserializeResponseAsync<GitPullRequestQuery<T>>();
        
        public async Task<WorkItem> UpdateWorkItemFieldAsync(int workItemId, string fieldName, string value)
        {
            var operations = new List<Operation>
                                 {
                                     new Operation {op = "replace", path = $"/fields/{fieldName}", value = value}
                                 };
            var jsonBody = JsonConvert.SerializeObject(operations);

            return await _factory.Create()
                                .WithPath($"_apis/wit/workitems/{workItemId}")
                                .Patch(jsonBody)
                                .DeserializeResponseAsync<WorkItem>();
        }

        private readonly IAzureDevOpsUrlBuilderFactory _factory;
    }
}