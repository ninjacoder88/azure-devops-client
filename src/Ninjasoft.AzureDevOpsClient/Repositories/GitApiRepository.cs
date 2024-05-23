using Newtonsoft.Json;
using Ninjasoft.AzureDevOpsClient.Models;

namespace Ninjasoft.AzureDevOpsClient.Repositories
{
    public class GitApiRepository
    {
        public GitApiRepository(IAzureDevOpsUrlBuilderFactory factory)
        {
            _factory = factory;
        }

        public async Task<List<GitPullRequest>> GetActivePullRequestsForRepositoryAsync(string repositoryId) =>
            await _factory.Create()
                            .WithPath($"_apis/git/repositories/{Uri.EscapeDataString(repositoryId)}/pullrequests")
                            .WithQueryString("searchCriteria.status=active")
                            .Get()
                            .DeserializeResponseListAsync<GitPullRequest>();

        public async Task<List<ResourceRef>> GetPullRequestWorkItemsAsync(string repositoryId, int pullRequestId) =>
            await _factory.Create()
                            .WithPath($"_apis/git/repositories/{repositoryId}/pullRequests/{pullRequestId}/workitems")
                            .Get()
                            .DeserializeResponseListAsync<ResourceRef>();

        public async Task<List<GitRepository>> GetRepositoriesForProjectAsync() =>
            await _factory.Create()
                            .WithPath("/_apis/git/repositories")
                            .Get()
                            .DeserializeResponseListAsync<GitRepository>();

        public async Task<GitPullRequestQuery<T>> PullRequestQueryAsync<T>(string repositoryId, GitPullRequestQueryInputList input) =>
            await _factory.Create()
                            .WithPath($"_apis/git/repositories/{repositoryId}/pullrequestquery")
                            .Post(JsonConvert.SerializeObject(input))
                            .DeserializeResponseAsync<GitPullRequestQuery<T>>();

        private readonly IAzureDevOpsUrlBuilderFactory _factory;
    }
}
