namespace Ninjasoft.AzureDevOpsClient.Models
{
	public class TeamSettingsIteration
    {
        public string id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public TeamIterationAttributes attributes { get; set; }
    }
}