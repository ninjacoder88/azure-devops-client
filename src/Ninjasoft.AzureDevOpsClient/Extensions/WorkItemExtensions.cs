using Ninjasoft.AzureDevOpsClient.Models;
using Newtonsoft.Json.Linq;

namespace Ninjasoft.AzureDevOpsClient.Extensions
{
    public static class WorkItemExtensions
    {
        public static T? GetFieldValue<T>(this WorkItem workItem, string fieldName) where T : class
        {
            if (!workItem.Fields.TryGetValue(fieldName, out object? fieldObject))
                return default;

            if (fieldObject is JObject jObject)
                return jObject.ToObject<T>();

            return fieldObject as T;
        }

        public static T? GetFieldValueAsStruct<T>(this WorkItem workItem, string fieldName) where T : struct
        {
            if (!workItem.Fields.TryGetValue(fieldName, out object? fieldObject))
                return default;

            if (fieldObject is JObject jObject)
                return jObject.ToObject<T>();

            return (T)fieldObject;
        }

        public static string? GetFieldValueAsString(this WorkItem workItem, string fieldName)
        {
            if (!workItem.Fields.TryGetValue(fieldName, out object? fieldObject))
                return default;

            return fieldObject.ToString();
        }

        public static DateTime? GetFieldValueAsDateTime(this WorkItem workItem, string fieldName)
        {
            string? fieldValue = workItem.GetFieldValueAsString(fieldName);
            if(DateTime.TryParse(fieldValue, out DateTime dateTime))
                return dateTime;
            return null;
        }

        public static decimal? GetFieldValueAsDecimal(this WorkItem workItem, string fieldName)
        {
            string? fieldValue = workItem.GetFieldValueAsString(fieldName);

            if (decimal.TryParse(fieldValue, out decimal value))
                return value;
            return null;
        }
    }
}