namespace Ninjasoft.AzureDevOpsClient.Models
{
    public class GitPullRequestQueryInputList
    {
        public GitPullRequestQueryInputList()
        {
            Queries = new List<GitPullRequestQueryInput>();
        }

        public List<GitPullRequestQueryInput> Queries { get; set; }
    }
}
