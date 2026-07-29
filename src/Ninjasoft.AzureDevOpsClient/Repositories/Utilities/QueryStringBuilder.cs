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

        public QueryStringBuilder Set(string key, decimal value) => Set(key, value.ToString());

        public QueryStringBuilder Set(string key, bool value) => Set(key, value.ToString());

        public QueryStringBuilder Set(string key, DateTime value, string? format = null)
        {
            string str;
            if(!string.IsNullOrWhiteSpace(format))
                str = value.ToString(format);
            else
                str = value.ToString("yyyy-MM-ddThh:mm:ss.fffZ");
            return Set(key, str);
        }

        public string Build() => string.Join("&", _queryParameters.Select(x => $"{x.Key}={x.Value}"));

        private readonly Dictionary<string, string> _queryParameters;
    }
}
