using Ninjasoft.AzureDevOpsClient.Models;

namespace Ninjasoft.AzureDevOpsClient.Extensions
{
    public static class PullRequestQueryResultExtensions
    {
        public static IEnumerable<int> ExtractPullRequestIds(this GitPullRequestQuery<PullRequestQueryResult> pullRequestQueryResult)
        {
            foreach(var prqr in pullRequestQueryResult.Results)
            {
                foreach(var kvp in prqr)
                {
                    foreach(var item in kvp.Value)
                    {
                        object prIdObj = item["pullRequestId"];
                        string prIdStr = prIdObj?.ToString() ?? string.Empty;
                        if (int.TryParse(prIdStr, out var prId))
                            yield return prId;
                    }
                }
            }
        }
    }
}
