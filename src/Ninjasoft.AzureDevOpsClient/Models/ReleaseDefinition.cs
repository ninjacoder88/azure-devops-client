using System.Text.Json.Serialization;

namespace Ninjasoft.AzureDevOpsClient.Models
{
    public class ReleaseDefinition
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;

        public string Path { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("_links")]
        public Links Links { get; set; }
        
        public List<ReleaseDefinitionEnvironment> Environments { get; set; }
    }

    public class ReleaseDefinitionEnvironment
    {
        public string Name { get; set; } = string.Empty;
        
        public int Id { get; set; }
        
        public List<ReleaseDefinitionDeployPhase> DeployPhases { get; set; }
    }

    public class ReleaseDefinitionDeployPhase
    {
        public string Name { get; set; } = string.Empty;

        public List<WorkflowTask> WorkflowTasks { get; set; }
    }
    
    public class WorkflowTask
    {
        public string Name { get; set; } = string.Empty;
    }
}