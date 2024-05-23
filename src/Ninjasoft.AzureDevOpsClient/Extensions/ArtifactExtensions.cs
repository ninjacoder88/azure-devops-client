using Ninjasoft.AzureDevOpsClient.Models;

namespace Ninjasoft.AzureDevOpsClient.Extensions
{
    public static class ArtifactExtensions
    {
        public static string? GetArtifactSourceReferenceId(this Artifact artifact, string key)
        {
            var source = artifact.GetArtifactSourceReference(key);
            if (source == null)
                return null;

            return source.Id;
        }

        public static string? GetArtifactSourceReferenceName(this Artifact artifact, string key)
        {
            var source = artifact.GetArtifactSourceReference(key);
            if (source == null)
                return null;

            return source.Name;
        }

        public static ArtifactSourceReference? GetArtifactSourceReference(this Artifact artifact, string key)
        {
            if (!artifact.DefinitionReference.TryGetValue(key, out ArtifactSourceReference? artifactSourceReference))
                return null;

            return artifactSourceReference;
        }
    }
}
