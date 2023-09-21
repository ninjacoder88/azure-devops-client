using System.Collections.Generic;

namespace Ninjasoft.AzureDevOpsClient.Models
{
    public class WorkItemRelation
    {
        public string Rel { get; set; }
        
        public string Url { get; set; }
        
        public Dictionary<string, string> Attributes { get; set; }
    }
}