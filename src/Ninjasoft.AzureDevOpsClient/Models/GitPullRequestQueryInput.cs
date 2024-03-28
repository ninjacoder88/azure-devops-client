using Ninjasoft.AzureDevOpsClient.Constants;

namespace Ninjasoft.AzureDevOpsClient.Models
{
    public class GitPullRequestQueryInput
    {
        public GitPullRequestQueryInput()
        {
            Items = new List<string>();
        }

        public List<string> Items { get; set; }

        public GitPullRequestQueryType Type { get; set; }
    }
}
