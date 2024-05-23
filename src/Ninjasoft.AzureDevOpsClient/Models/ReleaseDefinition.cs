using System.Collections.Generic;

namespace Ninjasoft.AzureDevOpsClient.Models
{
    public class ReleaseDefinition
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Path { get; set; }
        
        public List<ReleaseDefinitionEnvironment> Environments { get; set; }
    }

    public class ReleaseDefinitionEnvironment
    {
        public string Name { get; set; }
        
        public int Id { get; set; }
        
        public List<ReleaseDefinitionDeployPhase> DeployPhases { get; set; }
    }

    public class ReleaseDefinitionDeployPhase
    {
        public string Name { get; set; }
        
        public List<WorkflowTask> WorkflowTasks { get; set; }
    }
    
    public class WorkflowTask
    {
        public string Name { get; set; }
    }
}