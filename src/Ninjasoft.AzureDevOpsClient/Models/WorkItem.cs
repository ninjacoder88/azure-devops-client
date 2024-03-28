using System.Collections.Generic;

namespace Ninjasoft.AzureDevOpsClient.Models
{
    public class WorkItem
    {
        public int Id { get; set; }
        public Dictionary<string, object> Fields { get; set; }
        public List<WorkItemRelation> Relations { get; set; }
    }
}