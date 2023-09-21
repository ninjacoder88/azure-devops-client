namespace Ninjasoft.AzureDevOpsClient.Models
{
    public enum Vote
    {
        rejected = -10,
        waitingforauthor = -5,
        novote = 0,
        approvedwithsuggestions = 5,
        approved = 10
    }
}