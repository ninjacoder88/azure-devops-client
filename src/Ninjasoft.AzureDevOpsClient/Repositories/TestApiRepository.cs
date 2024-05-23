using Ninjasoft.AzureDevOpsClient.Models;
using System.Globalization;


namespace Ninjasoft.AzureDevOpsClient.Repositories
{
    public class TestApiRepository
    {
        public TestApiRepository(IAzureDevOpsUrlBuilderFactory factory)
        {
            _factory = factory;
        }

        public async Task<List<TestCaseResult>> GetTestCaseResultsAsync(int testRunId) =>
            await _factory.Create()
                            .WithPath($"_apis/test/runs/{testRunId}/results")
                            .Get()
                            .DeserializeResponseListAsync<TestCaseResult>();

        public async Task<List<TestRun>> GetTestRunsAsync(DateTime start, DateTime end, int buildId) =>
            await _factory.Create()
                            .WithPath("_apis/test/runs")
                            .WithQueryString($"minLastUpdatedDate={start.ToString("s", DateTimeFormatInfo.CurrentInfo)}&maxLastUpdatedDate={end.ToString("s", DateTimeFormatInfo.CurrentInfo)}&buildIds={buildId}&$top=1")
                            .Get()
                            .DeserializeResponseListAsync<TestRun>();

        public async Task<List<TestRunCoverage>> GetTestRunCodeCoverageAsync(int testRunId) =>
            await _factory.Create()
                            .WithPath($"_apis/test/Runs/{testRunId}/codecoverage")
                            .WithQueryString("flags=1")
                            .Get()
                            .DeserializeResponseListAsync<TestRunCoverage>();

        public async Task<List<TestRunCoverage>> GetBuildCodeCoverageAsync(int buildId) =>
            await _factory.Create()
                            .WithPath("/_apis/test/codecoverage")
                            .WithQueryString($"buildId={buildId}&flags=1")
                            .Get()
                            .DeserializeResponseListAsync<TestRunCoverage>();

        private readonly IAzureDevOpsUrlBuilderFactory _factory;
    }
}
