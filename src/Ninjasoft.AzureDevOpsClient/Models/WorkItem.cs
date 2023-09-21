using System.Collections.Generic;

namespace Ninjasoft.AzureDevOpsClient.Models
{
    public class WorkItem
    {
        public int id { get; set; }
        public Dictionary<string, object> fields { get; set; }
        public List<WorkItemRelation> Relations { get; set; }
    }
}