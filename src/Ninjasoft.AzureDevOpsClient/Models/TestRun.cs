namespace Ninjasoft.AzureDevOpsClient.Models
{
    public class TestRun
    {
        public int Id { get; set; }

        public int TotalTests { get; set; }

        public int PassedTests { get; set; }
    }
}