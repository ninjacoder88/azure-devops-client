using System.Collections.Generic;

namespace Ninjasoft.AzureDevOpsClient.Models
{
    public class ResponseList<T>
    {
        public int Count { get; set; }
        public List<T> Value { get; set; }
    }
}