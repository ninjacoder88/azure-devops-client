using Newtonsoft.Json;

namespace Ninjasoft.AzureDevOpsClient.Extensions
{
    public static class AzureDevOpsUrlBuilderV2Extensions
    {
        public static async Task<string> Get(this AzureDevOpsUrlBuilderV2 builder, string personalAccessToken)
        {
            for(int i = 0; i < 5; i++)
            {
                await Task.Delay(i * 1000);
                try
                {
                    return await new Http(personalAccessToken).Get(builder.Build());
                }
                catch
                {
                    if (i == 4)
                        throw;
                }
            }
            return string.Empty;
        }

        public static async Task<string> PostAsync(this AzureDevOpsUrlBuilderV2 builder, string personalAccessToken, object input)
        {
            return await new Http(personalAccessToken).Post(builder.Build(), JsonConvert.SerializeObject(input));
        }

        public static async Task<string> PatchAsync(this AzureDevOpsUrlBuilderV2 builder, string personalAccessToken, object input)
        {
            return await new Http(personalAccessToken).PatchAsync(builder.Build(), JsonConvert.SerializeObject(input));
        }
    }
}