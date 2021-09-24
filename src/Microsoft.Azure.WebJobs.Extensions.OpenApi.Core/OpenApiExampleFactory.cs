using Microsoft.OpenApi.Any;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core
{
    /// <summary>
    /// This represents the factory entity to create a openApiExample instance based on the OpenAPI document format.
    /// </summary>
    public static class OpenApiExampleFactory
    {
        /// <summary>
        /// Creates a new instance of <see cref="IOpenApiAny"/> based on the OpenAPI document format.
        /// </summary>
        /// <param name="instance">instance.</param>
        /// <param name="settings"><see cref="JsonSerializerSettings"/>settings.</param>
        /// <returns><see cref="IOpenApiAny"/> instance.</returns>
        public static IOpenApiAny CreateInstance<T>(T instance, JsonSerializerSettings settings)
        {
            var openApiExampleValue = default(IOpenApiAny);

            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Int16:
                    openApiExampleValue = new OpenApiInteger(Convert.ToInt16(instance));
                    break;
                case TypeCode.Int32:
                    openApiExampleValue = new OpenApiInteger(Convert.ToInt32(instance));
                    break;
                case TypeCode.Int64:
                    openApiExampleValue = new OpenApiLong(Convert.ToInt64(instance));
                    break;
                case TypeCode.UInt16:
                    openApiExampleValue = new OpenApiInteger(Convert.ToUInt16(instance));
                    break;
                case TypeCode.UInt32:
                    openApiExampleValue = new OpenApiDouble(Convert.ToUInt32(instance));
                    break;
                case TypeCode.UInt64:
                    openApiExampleValue = new OpenApiDouble(Convert.ToUInt64(instance));
                    break;
                case TypeCode.Single:
                    openApiExampleValue = new OpenApiFloat(Convert.ToSingle(instance));
                    break;
                case TypeCode.Double:
                    openApiExampleValue = new OpenApiDouble(Convert.ToDouble(instance));
                    break;
                case TypeCode.Boolean:
                    openApiExampleValue = new OpenApiBoolean(Convert.ToBoolean(instance));
                    break;
                case TypeCode.String:
                    openApiExampleValue = new OpenApiString(Convert.ToString(instance));
                    break;
                case TypeCode.DateTime:
                    openApiExampleValue = new OpenApiDateTime(Convert.ToDateTime(instance));
                    break;
                case TypeCode.Object:
                    openApiExampleValue = new OpenApiString(JsonConvert.SerializeObject(instance, settings));
                    break;
                default:
                    throw new InvalidOperationException("Invalid OpenAPI data Format");
            }
            return openApiExampleValue;
        }
    }
}
