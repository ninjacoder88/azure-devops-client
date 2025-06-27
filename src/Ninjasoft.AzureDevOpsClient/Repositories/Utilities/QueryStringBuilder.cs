namespace Ninjasoft.AzureDevOpsClient.Repositories.Utilities
{
    public class QueryStringBuilder
    {
        public QueryStringBuilder()
        {
            _queryParameters = new Dictionary<string, string>();
        }

        public QueryStringBuilder Set(string key, string value)
        {
            _queryParameters[key] = value;
            return this;
        }

        public QueryStringBuilder Set(string key, int value) => Set(key, value.ToString());

        public string Build() => string.Join("&", _queryParameters.Select(x => $"{x.Key}={x.Value}"));

        private readonly Dictionary<string, string> _queryParameters;
    }
}
