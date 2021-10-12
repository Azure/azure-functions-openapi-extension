using System.Collections.Generic;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class GenericAndRecursiveObjectModel
    {
        public List<GenericAndRecursiveObjectModel> ListValue  { get; set; }
        public Dictionary<string, GenericAndRecursiveObjectModel> DictionaryValue { get; set; }

    }
}
