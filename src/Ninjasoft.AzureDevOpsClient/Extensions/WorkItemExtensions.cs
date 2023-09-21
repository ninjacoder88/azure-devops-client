using Ninjasoft.AzureDevOpsClient.Models;
using Newtonsoft.Json.Linq;

namespace Ninjasoft.AzureDevOpsClient.Extensions
{
    public static class WorkItemExtensions
    {
        public static T GetFieldValue<T>(this WorkItem workItem, string fieldName) where T : class
        {
            if (!workItem.fields.TryGetValue(fieldName, out object fieldObject))
                return default;

            var fieldObjectType = fieldObject.GetType();

            if (fieldObjectType == typeof(JObject))
            {
                var jobject = fieldObject as JObject;

                return jobject.ToObject<T>();
            }

            return fieldObject as T;
        }

        public static T GetFieldValueAsStruct<T>(this WorkItem workItem, string fieldName) where T : struct
        {
            if (!workItem.fields.TryGetValue(fieldName, out object fieldObject))
                return default;

            var fieldObjectType = fieldObject.GetType();

            if (fieldObjectType == typeof(JObject))
            {
                var jobject = fieldObject as JObject;

                return jobject.ToObject<T>();
            }

            return (T)fieldObject;
        }

        public static string GetFieldValueAsString(this WorkItem workItem, string fieldName)
        {
            if (!workItem.fields.TryGetValue(fieldName, out object fieldObject))
                return default;

            return fieldObject.ToString();
        }

        public static DateTime? GetFieldValueAsDateTime(this WorkItem workItem, string fieldName)
        {
            var fieldValue = workItem.GetFieldValueAsString(fieldName);

            return DateTime.Parse(fieldValue);
        }

        public static decimal? GetFieldValueAsDecimal(this WorkItem workItem, string fieldName)
        {
            var fieldValue = workItem.GetFieldValueAsString(fieldName);

            if (decimal.TryParse(fieldValue, out decimal value))
                return value;
            return null;
        }
    }
}