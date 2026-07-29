using Ninjasoft.AzureDevOpsClient.Repositories.Utilities;

namespace Ninjasoft.AzureDevOpsClient.UnitTests.QueryStringBuilderTests
{
    internal class BuildTests
    {
        [Test]
        public void BuildCalledWithNoSets_ReturnsEmptyString()
        {
            QueryStringBuilder SUT = new ();

            string result = SUT.Build();

            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void SetString_ReturnsQueryString()
        {
            QueryStringBuilder SUT = new();
            SUT.Set("key", "value");

            string result = SUT.Build();

            Assert.That(result, Is.EqualTo("key=value"));
        }

        [Test]
        public void SetInt_ReturnsQueryString()
        {
            QueryStringBuilder SUT = new();
            SUT.Set("key", 100);

            string result = SUT.Build();

            Assert.That(result, Is.EqualTo("key=100"));
        }

        [Test]
        public void SetDecimal_ReturnsQueryString()
        {
            QueryStringBuilder SUT = new();
            SUT.Set("key", 5.2m);

            string result = SUT.Build();

            Assert.That(result, Is.EqualTo("key=5.2"));
        }

        [Test]
        public void SetBool_ReturnsQueryString()
        {
            QueryStringBuilder SUT = new();
            SUT.Set("key", true);

            string result = SUT.Build();

            Assert.That(result, Is.EqualTo("key=True"));
        }

        [Test]
        public void SetDate_ReturnsQueryString()
        {
            DateTime d = new DateTime(2025, 11, 8, 2, 34, 56);
            QueryStringBuilder SUT = new();
            SUT.Set("key", d);

            string result = SUT.Build();

            Assert.That(result, Is.EqualTo("key=2025-11-08T02:34:56.000Z"));
        }

        [Test]
        public void SetMultiple_ReturnsQueryString()
        {
            QueryStringBuilder SUT = new();
            SUT.Set("keyStr", "value");
            SUT.Set("keyInt", 100);
            SUT.Set("keyBool", true);

            string result = SUT.Build();

            Assert.That(result, Is.EqualTo("keyStr=value&keyInt=100&keyBool=True"));
        }
    }
}
