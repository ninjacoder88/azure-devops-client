using MoreLinq;
using Newtonsoft.Json;
using Ninjasoft.AzureDevOpsClient.Models;

namespace Ninjasoft.AzureDevOpsClient.Repositories
{
    public class WorkItemTrackApiRepsository
    {
        public WorkItemTrackApiRepsository(IAzureDevOpsUrlBuilderFactory factory)
        {
            _factory = factory;
        }

        public async Task<List<WorkItem>> GetWorkItemsAsync(List<int> workItemIds)
        {
            var batchedWorkItemIds = workItemIds.Batch(200);

            var list = new List<WorkItem>();
            foreach (var batchOfWorkItemIds in batchedWorkItemIds)
            {
                string workItemIdsStrings = string.Join(",", batchOfWorkItemIds);
                var response = await _factory.Create()
                                             .WithPath("_apis/wit/workitems")
                                             .WithQueryString($"ids={workItemIdsStrings}")
                                             .Get()
                                             .DeserializeResponseListAsync<WorkItem>();
                list.AddRange(response);
            }

            return list;
        }

        public async Task<List<WorkItemUpdate>> GetWorkItemUpdatesAsync(int workItemId) =>
            await _factory.Create()
                            .WithPath($"_apis/wit/workitems/{workItemId}/updates")
                            .Get()
                            .DeserializeResponseListAsync<WorkItemUpdate>();

        public async Task<WorkItem> GetWorkItemAsync(int workItemId, bool includeRelations = false)
        {
            string queryString = "";
            if (includeRelations)
                queryString += "$expand=Relations";

            return await _factory.Create()
                                .WithPath($"_apis/wit/workitems/{workItemId}")
                                .WithQueryString(queryString)
                                .Get()
                                .DeserializeResponseAsync<WorkItem>();
        }

        public async Task<WorkItem> UpdateWorkItemFieldAsync(int workItemId, string fieldName, string value)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string> { { fieldName, value } };
            return await UpdateWorkItemFieldAsync(workItemId, dictionary);
        }

        public async Task<WorkItem> UpdateWorkItemFieldAsync(int workItemId, Dictionary<string, string> fieldDictionary)
        {
            var operations = fieldDictionary.Select(x => new Operation { op = "replace", path = $"/fields/{x.Key}", value = x.Value }).ToList();

            var jsonBody = JsonConvert.SerializeObject(operations);

            return await _factory.Create()
                                .WithPath($"_apis/wit/workitems/{workItemId}")
                                .Patch(jsonBody)
                                .DeserializeResponseAsync<WorkItem>();
        }

        private readonly IAzureDevOpsUrlBuilderFactory _factory;
    }
}
