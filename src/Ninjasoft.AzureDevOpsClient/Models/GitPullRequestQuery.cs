namespace Ninjasoft.AzureDevOpsClient.Models
{
    public class GitPullRequestQuery<T>
    {
        public List<GitPullRequestQueryInput> Queries { get; set; }

        public List<T> Results { get; set; }
    }
}
