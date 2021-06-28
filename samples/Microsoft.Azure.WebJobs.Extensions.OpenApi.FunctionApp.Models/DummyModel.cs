using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models
{
    public class DummyModel
    {
        [OpenApiProperty(Description = "The number of Dummy")]
        public int Number { get; set; }

        [OpenApiProperty(Description = "The text of Dummy")]
        public string Text { get; set; }
    }
}
