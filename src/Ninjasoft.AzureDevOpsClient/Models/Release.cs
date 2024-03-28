using System.Collections.Generic;

namespace Ninjasoft.AzureDevOpsClient.Models
{
    public class Release
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string CreatedOn { get; set; }
        
        public ReleaseDefinition ReleaseDefinition { get; set; }
        
        public List<Artifact> Artifacts { get; set; }

        public IdentityRef CreatedBy { get; set; }

        public ProjectReference ProjectReference { get; set; }

        public List<ReleaseEnvironment> Environments { get; set; }
    }
}