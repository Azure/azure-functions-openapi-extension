using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class JObjectModel
    {
        public JObject jObjectValue { get; set; }
        public JToken jTokenValue { get; set; }
        public JArray jArrayValue { get; set; }

    }
}
