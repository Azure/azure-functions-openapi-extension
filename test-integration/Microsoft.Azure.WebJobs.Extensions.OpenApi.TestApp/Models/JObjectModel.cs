using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class JObjectModel
    {
        public JObject JObjectValue { get; set; }
        public JToken JTokenValue { get; set; }
        public JArray JArrayValue { get; set; }

    }
}
