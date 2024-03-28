namespace Ninjasoft.AzureDevOpsClient.Models
{
    public class Build
    {
        public int Id { get; set; }

        public string StartTime { get; set; }

        public string FinishTime { get; set; }

        public IdentityRef RequestedFor { get; set; }
    }
}