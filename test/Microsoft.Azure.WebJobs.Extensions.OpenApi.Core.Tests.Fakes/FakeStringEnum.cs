using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    /// <summary>
    /// This specifies fake enum values as string.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FakeStringEnum
    {
        [Display("lorem")]
        StringValue1,

        [Display("ipsum")]
        StringValue2
    }
}
