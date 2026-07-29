using System.Text.Json;

namespace Ninjasoft.AzureDevOpsClient.Models
{
    public class WorkItemUpdate
    {
        public int Id { get; set; }

        public IdentityRef RevisedBy { get; set; } = null!;

        public string RevisedDate { get; set; } = string.Empty;

        public Dictionary<string, JsonElement> Fields { get; set; } = null!;
    }
}