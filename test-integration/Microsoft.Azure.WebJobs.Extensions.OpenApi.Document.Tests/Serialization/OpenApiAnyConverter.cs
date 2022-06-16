using Microsoft.OpenApi.Any;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Document.Tests.Serialization
{
    /// <summary>
    /// IOpenApiAny types inside OpenApiDocument can't be deserialized without knowing which concrete type to deserialize to.
    /// This custom converter deserializes json to OpenApiString type.
    /// </summary>
    public class OpenApiAnyConverter : JsonConverter<IOpenApiAny>
    {
        public override IOpenApiAny ReadJson(JsonReader reader, Type objectType, IOpenApiAny existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var s = (string)reader.Value;
            return new OpenApiString(s);
        }

        public override void WriteJson(JsonWriter writer, IOpenApiAny value, JsonSerializer serializer)
        {
            //not used
            var t = JToken.FromObject(value);
            t.WriteTo(writer);
        }
    }
}
