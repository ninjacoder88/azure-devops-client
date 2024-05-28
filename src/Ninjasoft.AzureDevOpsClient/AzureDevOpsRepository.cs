using Ninjasoft.AzureDevOpsClient.Extensions;
using Ninjasoft.AzureDevOpsClient.Models;
using Ninjasoft.AzureDevOpsClient.Repositories;

namespace Ninjasoft.AzureDevOpsClient
{
    public class AzureDevOpsRepository
    {
        public AzureDevOpsRepository(IAzureDevOpsUrlBuilderFactory factory)
        {
            _factory = factory;
        }

        public BuildApiRepository Build
        {
            get => _build ?? (_build = new BuildApiRepository(_factory));
            private set => _build = value;
        }

        public GitApiRepository Git
        {
            get => _git ?? (_git = new GitApiRepository(_factory));
            private set => _git = value;
        }

        public PipelinesApiRepository Pipelines
        {
            get => _pipelines ?? (_pipelines = new PipelinesApiRepository(_factory));
            private set => _pipelines = value;
        }

        public ProjectsApiRepository Projects
        {
            get => _projects ?? (_projects = new ProjectsApiRepository(_factory));
            private set => _projects = value;
        }

        public ReleaseApiRepository Release
        {
            get => _release ?? (_release = new ReleaseApiRepository(_factory));
            private set => _release = value;
        }

        public TestApiRepository Test
        {
            get => _test ?? (_test = new TestApiRepository(_factory));
            private set => _test = value;
        }

        public WorkApiRepository Work
        {
            get => _work ?? (_work =  new WorkApiRepository(_factory));
            private set => _work = value;
        }

        public WorkItemTrackApiRepsository WorkItemTracking
        {
            get => _workItemTracking ?? (_workItemTracking = new WorkItemTrackApiRepsository(_factory));
            private set => _workItemTracking = value;
        }

        public async Task<List<ResourceRef>> GetWorkItemsFromBuildAsync(int buildId, string repositoryId)
        {
            var buildChanges = await Build.GetBuildChangesAsync(buildId);

            var commitIds = buildChanges.Where(x => x.Type == "TfsGit").Select(x => x.Id).ToList();

            var pullRequestQueryList = new GitPullRequestQueryInputList
            {
                Queries = new List<GitPullRequestQueryInput>
                {
                    new GitPullRequestQueryInput
                    {
                        Type = Constants.GitPullRequestQueryType.Commit,
                        Items = commitIds
                    }
                }
            };

            var pullRequestQueryResult = await Git.PullRequestQueryAsync<PullRequestQueryResult>(repositoryId, pullRequestQueryList);

            List<ResourceRef> workItemResourceRefs = new List<ResourceRef>();
            foreach (var pullRequestId in pullRequestQueryResult.ExtractPullRequestIds())
            {
                var workItemRefs = await Git.GetPullRequestWorkItemsAsync(repositoryId, pullRequestId);
                workItemResourceRefs.AddRange(workItemRefs);
            }

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
        private readonly IAzureDevOpsUrlBuilderFactory _factory;
    }
}