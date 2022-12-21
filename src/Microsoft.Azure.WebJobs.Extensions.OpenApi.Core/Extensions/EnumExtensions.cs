using System;
using System.Linq;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for enums.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the display name of the enum value.
        /// </summary>
        /// <param name="enum">Enum value.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance.</param>
        /// <returns>Display name of the enum value.</returns>
        public static string ToString(this Enum @enum, NamingStrategy namingStrategy = null)
        {
            return @enum.ToDisplayName(namingStrategy);
        }

        /// <summary>
        /// Gets the display name of the enum value.
        /// </summary>
        /// <param name="enum">Enum value.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance.</param>
        /// <returns>Display name of the enum value.</returns>
        public static string ToDisplayName(this Enum @enum, NamingStrategy namingStrategy = null)
        {
            if (namingStrategy.IsNullOrDefault())
            {
                namingStrategy = new DefaultNamingStrategy();
            }

            var type = @enum.GetType();
            var member = type.GetMember(@enum.ToString()).First();

            return member.ToDisplayName(namingStrategy);
        }

        /// <summary>
        /// Converts the <see cref="TypeCode"/> value into data type specified in OpenAPI spec.
        /// </summary>
        /// <param name="type"><see cref="Type"/> value.</param>
        /// <returns>Data type specified in OpenAPI spec.</returns>
        public static string ToDataType(this Type type)
        {
            var @enum = Type.GetTypeCode(type);
            if (!type.IsNullOrDefault() && type.IsEnum)
            {
                @enum = TypeCode.String;
            }

            switch (@enum)
            {
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return "integer";

                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return "number";

                case TypeCode.Boolean:
                    return "boolean";

                case TypeCode.DateTime:
                case TypeCode.String:
                    return "string";

                case TypeCode.Object:
                    if (type == typeof(Guid))
                    {
                        return "string";
                    }
                    else if (type == typeof(DateTime))
                    {
                        return "string";
                    }
                    else if (type == typeof(DateTimeOffset))
                    {
                        return "string";
                    }
                    else if (type == typeof(TimeSpan))
                    {
                        return "string";
                    }
                    else
                    {
                        return "object";
                    }

                case TypeCode.Empty:
                case TypeCode.DBNull:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                default:
                    throw new InvalidOperationException("Invalid data type");
            }
        }

        /// <summary>
        /// Converts the <see cref="TypeCode"/> value into data format specified in OpenAPI spec.
        /// </summary>
        /// <param name="type"><see cref="Type"/> value.</param>
        /// <returns>Data format specified in OpenAPI spec.</returns>
        public static string ToDataFormat(this Type type)
        {
            var @enum = Type.GetTypeCode(type);
            if (!type.IsNullOrDefault() && type.IsEnum)
            {
                @enum = TypeCode.String;
            }

            switch (@enum)
            {
                case TypeCode.Int16:
                case TypeCode.Int32:
                    return "int32";

                case TypeCode.Int64:
                    return "int64";

                case TypeCode.Single:
                    return "float";

                case TypeCode.Double:
                case TypeCode.Decimal:
                    return "double";

                case TypeCode.DateTime:
                    return "date-time";

                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Boolean:
                case TypeCode.String:
                    return null;

                case TypeCode.Object:
                    if (type == typeof(Guid))
                    {
                        return "uuid";
                    }
                    else if (type == typeof(DateTime))
                    {
                        return "date-time";
                    }
                    else if (type == typeof(DateTimeOffset))
                    {
                        return "date-time";
                    }
                    else if (type == typeof(TimeSpan))
                    {
                        return "timespan";
                    }
                    else
                    {
                        return null;
                    }

                case TypeCode.Empty:
                case TypeCode.DBNull:
                default:
                    throw new InvalidOperationException("Invalid data type");
            }
        }

        /// <summary>
        /// Gets the content type.
        /// </summary>
        /// <param name="format"><see cref="OpenApiFormat"/> value.</param>
        /// <returns>The content type.</returns>
        public static string GetContentType(this OpenApiFormat format)
        {
            switch (format)
            {
                case OpenApiFormat.Json:
                    return "application/json";

                case OpenApiFormat.Yaml:
                    // https://mailarchive.ietf.org/arch/msg/media-types/e9ZNC0hDXKXeFlAVRWxLCCaG9GI/
                    // "application/x-yaml", "text/yaml", "text/x-yaml" are deprecated.
                    return "text/vnd.yaml";

                default:
                    throw new InvalidOperationException("Invalid OpenAPI format");
            }
        }

        /// <summary>
        /// Converts the <see cref="OpenApiVersionType"/> to <see cref="OpenApiSpecVersion"/>.
        /// </summary>
        /// <param name="version"><see cref="OpenApiVersionType"/> value.</param>
        /// <returns>Returns <see cref="OpenApiSpecVersion"/> value.</returns>
        public static OpenApiSpecVersion ToOpenApiSpecVersion(this OpenApiVersionType version)
        {
            var casted = (OpenApiSpecVersion)(int)version;

            return casted;
        }

        /// <summary>
        /// Converts the <see cref="OpenApiFormatType"/> to <see cref="OpenApiFormat"/>.
        /// </summary>
        /// <param name="format"><see cref="OpenApiFormatType"/> value.</param>
        /// <returns>Returns <see cref="OpenApiFormat"/> value.</returns>
        public static OpenApiFormat ToOpenApiFormat(this OpenApiFormatType format)
        {
            var casted = (OpenApiFormat)(int)format;

            return casted;
        }
    }
}
