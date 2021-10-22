using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    /// <summary>
    /// This represents the fake model entity.
    /// </summary>
    public class FakeModel
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [JsonRequired]
        public string FakeProperty { get; set; }

        /// <summary>
        /// Gets or sets the value 2.
        /// </summary>
        [JsonProperty("anotherJsonFakeProperty", Required = Required.Always)]
        public string FakeProperty2 { get; set; }

        [JsonProperty(Required = Required.Default)]
        public string FakePropertyNoPropertyValue { get; set; }

        [JsonProperty]
        public string FakePropertyNoAnnotation { get; set; }

        [DataMember(Name = "anotherDataMemberFakeProperty")]
        public string FakeProperty3 { get; set; }

        [DataMember(Name = "dataMemberFakeProperty")]
        [JsonProperty("jsonFakeProperty")]
        public string FakeProperty4 { get; set; }

        public object FakeObject { get; set; }

        /// <summary>
        /// Gets or sets the nullable int value.
        /// </summary>
        public int? NullableInt { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="FakeSubModel"/> instance.
        /// </summary>
        public FakeSubModel SubProperty { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="FakeSubModel"/> instance.
        /// </summary>
        public FakeSubModel SubProperty2 { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="FakeStringEnum"/> value.
        /// </summary>
        public FakeStringEnum EnumProperty { get; set; }

        public FakeGenericModel<FakeClassModel> FakeGenericModelFakeClass { get; set; }
    }
}
