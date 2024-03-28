namespace Ninjasoft.AzureDevOpsClient.Models
{
    public class ReleaseApproval
    {
        public int Id { get; set; }

        public string CreatedOn { get; set; }

        public IdentityRef ApprovedBy { get; set; }

        public string Status { get; set; }
    }
}
