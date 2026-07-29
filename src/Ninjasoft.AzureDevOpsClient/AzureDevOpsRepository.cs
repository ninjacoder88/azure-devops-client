using Ninjasoft.AzureDevOpsClient.Extensions;
using Ninjasoft.AzureDevOpsClient.Models;
using Ninjasoft.AzureDevOpsClient.Repositories;

namespace Ninjasoft.AzureDevOpsClient
{
    public interface IAzureDevOpsRepository
    {
        BuildApiRepository Build { get; }

        GitApiRepository Git { get; }

        PipelinesApiRepository Pipelines { get; }

        ProjectsApiRepository Projects { get; }

        ReleaseApiRepository Release { get; }

        TestApiRepository Test { get; }

        WorkApiRepository Work { get; }

        WorkItemTrackApiRepsository WorkItemTracking { get; }

        Task<List<ResourceRef>> GetWorkItemsFromBuildAsync(int buildId, string repositoryId);
    }

    public class AzureDevOpsRepository(IAzureDevOpsUrlBuilderFactory _factory) : IAzureDevOpsRepository
    {
        public AzureDevOpsRepository(string personalAccessToken, string organization, string projectName = "")
            : this(new AzureDevOpsUrlBuilderFactory(personalAccessToken, organization, projectName))
        {
        }

        public BuildApiRepository Build
        {
            get => _build ??= new BuildApiRepository(_factory);
            private set => _build = value;
        }

        public GitApiRepository Git
        {
            get => _git ??= new GitApiRepository(_factory);
            private set => _git = value;
        }

        public PipelinesApiRepository Pipelines
        {
            get => _pipelines ??= new PipelinesApiRepository(_factory);
            private set => _pipelines = value;
        }

        public ProjectsApiRepository Projects
        {
            get => _projects ??= new ProjectsApiRepository(_factory);
            private set => _projects = value;
        }

        public ReleaseApiRepository Release
        {
            get => _release ??= new ReleaseApiRepository(_factory);
            private set => _release = value;
        }

        public TestApiRepository Test
        {
            get => _test ??= new TestApiRepository(_factory);
            private set => _test = value;
        }

        public WorkApiRepository Work
        {
            get => _work ??=  new WorkApiRepository(_factory);
            private set => _work = value;
        }

        public WorkItemTrackApiRepsository WorkItemTracking
        {
            get => _workItemTracking ?? (_workItemTracking = new WorkItemTrackApiRepsository(_factory));
            private set => _workItemTracking = value;
        }

        public async Task<List<ResourceRef>> GetWorkItemsFromBuildAsync(int buildId, string repositoryId)
        {
            List<Change> buildChanges = await Build.GetBuildChangesAsync(buildId);

            List<string> commitIds = buildChanges.Where(x => x.Type == "TfsGit").Select(x => x.Id).ToList();

            GitPullRequestQueryInputList pullRequestQueryList = new()
            {
                Queries =
                [
                    new GitPullRequestQueryInput
                    {
                        Type = Constants.GitPullRequestQueryType.Commit,
                        Items = commitIds
                    }
                ]
            };

            var pullRequestQueryResult = await Git.PullRequestQueryAsync<PullRequestQueryResult>(repositoryId, pullRequestQueryList);

            List<ResourceRef> workItemResourceRefs = [];
            foreach (int pullRequestId in pullRequestQueryResult.ExtractPullRequestIds())
                workItemResourceRefs.AddRange(await Git.GetPullRequestWorkItemsAsync(repositoryId, pullRequestId));

            return workItemResourceRefs;
        }

        private GitApiRepository? _git;
        private BuildApiRepository? _build;
        private ReleaseApiRepository? _release;
        private WorkItemTrackApiRepsository? _workItemTracking;
        private TestApiRepository? _test;
        private WorkApiRepository? _work;
        private PipelinesApiRepository? _pipelines;
        private ProjectsApiRepository? _projects;
    }
}