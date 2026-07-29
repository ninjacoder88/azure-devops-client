using AutoFixture;

namespace Ninjasoft.AzureDevOpsClient.UnitTests.AzureDevOpsUrlBuilderTests
{
    internal class BuildTests
    {
        public BuildTests()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void BuildCalledWithNoSets_ReturnsBasicUrlWithVersionString()
        {
            string organization = _fixture.Create<string>();
            AzureDevOpsUrlBuilderV2 SUT = new(organization, null);

            string result = SUT.Build();

            Assert.That(result, Is.EqualTo($"https://dev.azure.com/{organization}/?api-version=6.0"));
        }

        [Test]
        public void OverrideApiVersion_ReturnsBasicUrlWithUpdatedVersionString()
        {
            string organization = _fixture.Create<string>();
            AzureDevOpsUrlBuilderV2 SUT = new(organization, null, "2.0");

            string result = SUT.Build();

            Assert.That(result, Is.EqualTo($"https://dev.azure.com/{organization}/?api-version=2.0"));
        }

        [Test]
        public void SetSubDomain_ReturnsUrlWithSubDomain()
        {
            string organization = _fixture.Create<string>();
            AzureDevOpsUrlBuilderV2 SUT = new(organization, null);
            SUT.SetSubDomain("vsrm");

            string result = SUT.Build();

            Assert.That(result, Is.EqualTo("https://vsrm.dev.azure.com/{organization}/?api-version=6.0"));
        }

        [Test]
        public void SetSpecificPath_ReturnsUrlWithSubDomain()
        {
            string organization = _fixture.Create<string>();
            AzureDevOpsUrlBuilderV2 SUT = new(organization, null);
            SUT.SetPath("build");

            string result = SUT.Build();

            Assert.That(result, Is.EqualTo("https://dev.azure.com/{organization}/build?api-version=6.0"));
        }

        [Test]
        public void SetPathWithBuilder_ReturnsUrlWithSubDomain()
        {
            string organization = _fixture.Create<string>();
            string project = _fixture.Create<string>();
            AzureDevOpsUrlBuilderV2 SUT = new(organization, project);

            string result = SUT.Build();

            Assert.That(result, Is.EqualTo($"https://dev.azure.com/{organization}/{project}/BUILDS?api-version=6.0"));
        }

        [Test]
        public void SetSpecificQueryString_ReturnsUrlWithSubDomain()
        {
            string organization = _fixture.Create<string>();
            string project = _fixture.Create<string>();
            AzureDevOpsUrlBuilderV2 SUT = new(organization, project);
            SUT.SetQueryString("key=value");

            string result = SUT.Build();

            Assert.That(result, Is.EqualTo("https://dev.azure.com/?api-version=6.0&key=value"));
        }

        [Test]
        public void SetQueryStringWithBuilder_ReturnsUrlWithSubDomain()
        {
            string organization = _fixture.Create<string>();
            string project = _fixture.Create<string>();
            AzureDevOpsUrlBuilderV2 SUT = new(organization, project);
            SUT.SetQueryString("key=value");

            string result = SUT.Build();

            Assert.That(result, Is.EqualTo("https://dev.azure.com/?api-version=6.0&key=value"));
        }

        private readonly Fixture _fixture;
    }
}
