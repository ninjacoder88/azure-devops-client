using System.Collections.Generic;

namespace Ninjasoft.AzureDevOpsClient.Models
{
    public class Artifact
    {
        public string Alias { get; set; }

        public Dictionary<string, ArtifactSourceReference> DefinitionReference { get; set; }
    }
}