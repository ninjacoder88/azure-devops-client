namespace Ninjasoft.AzureDevOpsClient.Models
{
    public class WorkItemLink
    {
        public string rel { get; set; }
        
        public WorkItemReference source { get; set; }
        
        public WorkItemReference target { get; set; }
    }
}