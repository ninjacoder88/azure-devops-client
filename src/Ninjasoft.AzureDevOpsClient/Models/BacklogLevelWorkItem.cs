namespace Ninjasoft.AzureDevOpsClient.Models
{
    public class BacklogLevelWorkItem
    {
        public string? Rel { get; set; }
        public WorkItemReference? Source { get; set; }
        public WorkItemReference? Target { get; set; }
    }
}
