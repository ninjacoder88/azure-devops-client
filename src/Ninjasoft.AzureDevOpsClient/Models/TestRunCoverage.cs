using System.Collections.Generic;

namespace Ninjasoft.AzureDevOpsClient.Models
{
    public class TestRunCoverage
    {
        public string State { get; set; }

        public List<ModuleCoverage> Modules { get; set; }
    }

    public class ModuleCoverage
    {
        public int BlockCount { get; set; }

        public string Name { get; set; }

        public CoverageStatistics Statistics { get; set; }
    }

    public class CoverageStatistics
    {
        public int BlocksCovered { get; set; }

        public int BlocksNotCovered { get; set; }

        public int LinesCovered { get; set; }

        public int LinesNotCovered { get; set; }

        public int LinesPartiallyCovered { get; set; }
    }
}