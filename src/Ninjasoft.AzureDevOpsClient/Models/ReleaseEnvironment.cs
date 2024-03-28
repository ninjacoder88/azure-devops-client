namespace Ninjasoft.AzureDevOpsClient.Models
{
    public class ReleaseEnvironment
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CreatedOn { get; set; }

        public List<ReleaseApproval> PreDeployApprovals { get; set; }
    }
}
