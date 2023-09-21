using MoreLinq;
using Newtonsoft.Json;
using Ninjasoft.AzureDevOpsClient.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninjasoft.AzureDevOpsClient
{
    public class AzureDevOpsRepository
    {
        private const string DefaultApiVersion = "6.0";

        public AzureDevOpsRepository(IAzureDevOpsUrlBuilderFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<GitPullRequest>> GetActivePullRequestsForRepositoryAsync(string repositoryId)
        {
            var response = await _factory.Create()
                                         .WithPath($"_apis/git/repositories/{repositoryId}/pullrequests")
                                         .WithQueryString("searchCriteria.status=active")
                                         .UsingApiVersion(DefaultApiVersion)
                                         .Get()
                                         .DeserializeResponseAsync<ResponseList<GitPullRequest>>();

            return response.Value;
        }

        public async Task<Build> GetBuildAsync(int buildId)
        {
            return await _factory.Create()
                                 .WithPath($"_apis/build/builds/{buildId}")
                                 .UsingApiVersion(DefaultApiVersion)
                                 .Get()
                                 .DeserializeResponseAsync<Build>();
        }

        public async Task<List<TestRunCoverage>> GetBuildCodeCoverageAsync(int buildId)
        {
            var response = await _factory.Create()
                                         .WithPath("/_apis/test/codecoverage")
                                         .WithQueryString($"buildId={buildId}&flags=1")
                                         .UsingApiVersion(DefaultApiVersion)
                                         .Get()
                                         .DeserializeResponseAsync<ResponseList<TestRunCoverage>>();

            return response.Value;
        }

        public async Task<List<TeamSettingsIteration>> GetIterationsAsync()
        {
            var response = await _factory.Create()
                                         .WithPath("active/_apis/work/teamsettings/iterations")
                                         .UsingApiVersion(DefaultApiVersion)
                                         .Get()
                                         .DeserializeResponseAsync<ResponseList<TeamSettingsIteration>>();
            return response.Value;
        }

        public async Task<IterationWorkItems> GetIterationWorkItemsAsync(string iterationId)
        {
            var response = await _factory.Create()
                                         .WithPath($"active/_apis/work/teamsettings/iterations/{iterationId}/workitems")
                                         .UsingApiVersion($"{DefaultApiVersion}-preview.1")
                                         .Get()
                                         .DeserializeResponseAsync<IterationWorkItems>();
            return response;
        }

        public async Task<IEnumerable<TeamProjectReference>> GetProjectsForOrganization()
        {
            var response = await _factory.Create()
                                         .WithPath("_apis/projects")
                                         .UsingApiVersion(DefaultApiVersion)
                                         .Get()
                                         .DeserializeResponseAsync<ResponseList<TeamProjectReference>>();

            return response.Value;
        }

        public async Task<Release> GetReleaseAsync(int releaseId)
        {
            var response = await _factory.Create()
                                         .WithSubDomain("vsrm")
                                         .WithPath($"_apis/release/releases/{releaseId}")
                                         .UsingApiVersion(DefaultApiVersion)
                                         .Get()
                                         .DeserializeResponseAsync<Release>();
            return response;
        }

        public async Task<List<ReleaseDefinition>> GetReleaseDefinitionsAsync()
        {
            var response = await _factory.Create()
                                         .WithSubDomain("vsrm")
                                         .WithPath("_apis/release/definitions")
                                         //.WithQueryString("$expand=Environments")
                                         .UsingApiVersion(DefaultApiVersion)
                                         .Get()
                                         .DeserializeResponseAsync<ResponseList<ReleaseDefinition>>();
            return response.Value;
        }

        public async Task<ReleaseDefinition> GetReleaseDefinitionAsync(int releaseDefinitionId)
        {
            var response = await _factory.Create()
                                         .WithSubDomain("vsrm")
                                         .WithPath($"_apis/release/definitions/{releaseDefinitionId}")
                                         .UsingApiVersion(DefaultApiVersion)
                                         .Get()
                                         .DeserializeResponseAsync<ReleaseDefinition>();
            return response;
        }

        public async Task<string> GetReleaseDefinitionStringAsync(int releaseDefinitionId)
        {
            var response = await _factory.Create()
                                         .WithSubDomain("vsrm")
                                         .WithPath($"_apis/release/definitions/{releaseDefinitionId}")
                                         .UsingApiVersion(DefaultApiVersion)
                                         .GetAsync();
            return response;
        }

        public async Task<List<Release>> GetReleasesAfterAsync(DateTimeOffset createdAfterDateTime)
        {
            string minCreatedTime = createdAfterDateTime.ToString("yyyy-MM-ddThh:mm:ss.fffZ");

            var response = await _factory.Create()
                                         .WithSubDomain("vsrm")
                                         .WithPath("_apis/release/releases")
                                         .UsingApiVersion(DefaultApiVersion)
                                         .WithQueryString($"minCreatedTime={minCreatedTime}")
                                         .Get()
                                         .DeserializeResponseAsync<ResponseList<Release>>();

            return response.Value;
        }

        public async Task<IEnumerable<GitRepository>> GetRepositoriesForProjectAsync()
        {
            var response = await _factory.Create()
                                         .WithPath("/_apis/git/repositories")
                                         .UsingApiVersion(DefaultApiVersion)
                                         .Get()
                                         .DeserializeResponseAsync<ResponseList<GitRepository>>();

            return response.Value;
        }

        public async Task<TestRun> GetTestRunAsync(DateTime start, DateTime end, int buildId)
        {
            var minLastUpdatedDate = start.ToString("s", DateTimeFormatInfo.CurrentInfo);
            var maxLastUpdatedDate = end.ToString("s", DateTimeFormatInfo.CurrentInfo);

            var response = await _factory.Create()
                                         .WithPath("_apis/test/runs")
                                         .WithQueryString($"minLastUpdatedDate={minLastUpdatedDate}&maxLastUpdatedDate={maxLastUpdatedDate}&buildIds={buildId}&$top=1")
                                         .UsingApiVersion(DefaultApiVersion)
                                         .Get()
                                         .DeserializeResponseAsync<ResponseList<TestRun>>();

            return response.Value.FirstOrDefault();
        }

        public async Task<List<TestRunCoverage>> GetTestRunCodeCoverage(int testRunId)
        {
            var response = await _factory.Create()
                                         .WithPath($"_apis/test/Runs/{testRunId}/codecoverage")
                                         .WithQueryString("flags=1")
                                         .UsingApiVersion(DefaultApiVersion)
                                         .Get()
                                         .DeserializeResponseAsync<ResponseList<TestRunCoverage>>();

            return response.Value;
        }

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
                                             .UsingApiVersion(DefaultApiVersion)
                                             .Get()
                                             .DeserializeResponseAsync<ResponseList<WorkItem>>();
                list.AddRange(response.Value);
            }

            return list;
        }

        public async Task<List<WorkItemUpdate>> GetWorkItemUpdatesAsync(int workItemId)
        {
            var response = await _factory.Create()
                                         .WithPath($"_apis/wit/workitems/{workItemId}/updates")
                                         .UsingApiVersion(DefaultApiVersion)
                                         .Get()
                                         .DeserializeResponseAsync<ResponseList<WorkItemUpdate>>();
            return response.Value;
        }

        public async Task<WorkItem> GetWorkItemAsync(int workItemId, bool includeRelations)
        {
            string queryString = "";
            if (includeRelations)
                queryString += "$expand=Relations";

            var response = await _factory.Create()
                                         .WithPath($"_apis/wit/workitems/{workItemId}")
                                         .WithQueryString(queryString)
                                         .UsingApiVersion(DefaultApiVersion)
                                         .Get()
                                         .DeserializeResponseAsync<WorkItem>();
            return response;
        }

        public async Task<List<Pipeline>> GetPipelinesAsync()
        {
            var response = await _factory.Create()
                                         .WithPath("_apis/pipelines")
                                         .UsingApiVersion(DefaultApiVersion)
                                         .Get()
                                         .DeserializeResponseAsync<ResponseList<Pipeline>>();
            return response.Value;
        }

        public async Task<WorkItem> UpdateWorkItemFieldAsync(int workItemId, string fieldName, string value)
        {
            var operations = new List<Operation>
                                 {
                                     new Operation {op = "replace", path = $"/fields/{fieldName}", value = value}
                                 };
            var jsonBody = JsonConvert.SerializeObject(operations);

            var response = await _factory.Create()
                                         .WithPath($"_apis/wit/workitems/{workItemId}")
                                         .UsingApiVersion(DefaultApiVersion)
                                         .Patch(jsonBody)
                                         .DeserializeResponseAsync<WorkItem>();
            return response;
        }

        private readonly IAzureDevOpsUrlBuilderFactory _factory;
    }

}
