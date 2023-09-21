using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Ninjasoft.AzureDevOpsClient.Models
{
    public class WorkItemUpdate
    {
        public int Id { get; set; }

        public IdentityRef RevisedBy { get; set; }

        public string RevisedDate { get; set; }

        public Dictionary<string, JObject> Fields { get; set; }
    }
}