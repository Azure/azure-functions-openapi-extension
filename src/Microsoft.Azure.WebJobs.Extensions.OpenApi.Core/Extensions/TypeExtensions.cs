using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.OpenApi.Any;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    ///  This represents the extension entity for <see cref="Type"/>.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Checks whether the given type is simple type or not.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Returns <c>True</c>, if simple type; otherwise returns <c>False</c>.</returns>
        public static bool IsSimpleType(this Type type)
        {
            var @enum = Type.GetTypeCode(type);
            switch (@enum)
            {
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.Boolean:
                case TypeCode.DateTime:
                case TypeCode.String:
                case TypeCode.Object when type == typeof(Guid):
                case TypeCode.Object when type == typeof(TimeSpan):
                case TypeCode.Object when type == typeof(DateTime):
                case TypeCode.Object when type == typeof(DateTimeOffset):
                    return true;

                case TypeCode.Empty:
                case TypeCode.DBNull:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                default:
                    return false;
            }
        }

        private static HashSet<Type> jObjects = new HashSet<Type>
        {
            typeof(JObject),
            typeof(JToken),
            typeof(JArray),
        };

        /// <summary>
        /// Checks whether the given type is Json.NET related <see cref="JObject"/>, <see cref="JToken"/> or not.
        /// </summary>
        /// <param name="type"><see cref="Type"/> instance.</param>
        /// <returns>Returns <c>True</c>, if the type is identified as either <see cref="JObject"/> or <see cref="JToken"/>; otherwise returns <c>False</c>.</returns>
        public static bool IsJObjectType(this Type type)
        {
            if (type.IsNullOrDefault())
            {
                return false;
            }

            if (jObjects.Any(p => p == type))
            {
                return true;
            }

            return false;
        }

        private static HashSet<Type> nonReferentials = new HashSet<Type>
        {
            typeof(Guid),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(Uri),
            typeof(object),
        };

        /// <summary>
        /// Checks whether the type can be referenced or not.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>Returns <c>True</c>, if the type can be referenced; otherwise returns <c>False</c>.</returns>

        public static bool IsReferentialType(this Type type)
        {
            var @enum = Type.GetTypeCode(type);
            var isReferential = @enum == TypeCode.Object;

            if (nonReferentials.Contains(type))
            {
                isReferential = false;
            }

            if (type.IsOpenApiNullable())
            {
                isReferential = false;
            }

            if (type.IsUnflaggedEnumType())
            {
                isReferential = false;
            }

            if (type.IsJObjectType())
            {
                isReferential = false;
            }

            return isReferential;
        }

        /// <summary>
        /// Checks whether the given type is enum without flags or not.
        /// </summary>
        /// /// <param name="type"><see cref="Type"/> instance.</param>
        /// <returns>Returns <c>True</c>, if the type is identified as enum without flags; otherwise returns <c>False</c>.</returns>
        public static bool IsEnumType(this Type type)
        {
            var isEnum = typeof(Enum).IsAssignableFrom(type);

            return isEnum;
        }

        /// <summary>
        /// Checks whether the given type is enum without flags or not.
        /// </summary>
        /// <param name="type"><see cref="Type"/> instance.</param>
        /// <returns>Returns <c>True</c>, if the type is identified as enum without flags; otherwise returns <c>False</c>.</returns>
        public static bool IsUnflaggedEnumType(this Type type)
        {
            var isEnum = type.IsEnumType();
            if (!isEnum)
            {
                return false;
            }

            var isFlagged = type.IsDefined(typeof(FlagsAttribute), false);
            if (isFlagged)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Converts the given <see cref="Type"/> instance to the list of underlying enum name.
        /// </summary>
        /// <param name="type"><see cref="Type"/> instance.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance.</param>
        /// <returns>Returns the list of underlying enum name.</returns>
        public static List<IOpenApiAny> ToOpenApiStringCollection(this Type type, NamingStrategy namingStrategy = null)
        {
            if (!type.IsUnflaggedEnumType())
            {
                return null;
            }

            // namingStrategy null check
            if (namingStrategy.IsNullOrDefault())
            {
                namingStrategy = new DefaultNamingStrategy();
            }
            // namingStrategy null check

            var members = type.GetMembers(BindingFlags.Public | BindingFlags.Static);
            var names = members.Select(p => p.ToDisplayName(namingStrategy));

            return names.Select(p => (IOpenApiAny)new OpenApiString(p))
                        .ToList();
        }

        /// <summary>
        /// Converts the given <see cref="Type"/> instance to the list of underlying enum values of <see cref="short"/>.
        /// </summary>
        /// <param name="type"><see cref="Type"/> instance.</param>
        /// <returns>Returns the list of underlying enum values of <see cref="short"/>.</returns>
        public static List<IOpenApiAny> ToOpenApiInt16Collection(this Type type)
        {
            if (!type.IsUnflaggedEnumType())
            {
                return null;
            }

            var names = Enum.GetValues(type).Cast<short>();

            return names.Select(p => (IOpenApiAny)new OpenApiInteger(Convert.ToInt32(p)))
                        .ToList();
        }

        /// <summary>
        /// Converts the given <see cref="Type"/> instance to the list of underlying enum values of <see cref="int"/>.
        /// </summary>
        /// <param name="type"><see cref="Type"/> instance.</param>
        /// <returns>Returns the list of underlying enum values of <see cref="int"/>.</returns>
        public static List<IOpenApiAny> ToOpenApiInt32Collection(this Type type)
        {
            if (!type.IsUnflaggedEnumType())
            {
                return null;
            }

            var names = Enum.GetValues(type).Cast<int>();

            return names.Select(p => (IOpenApiAny)new OpenApiInteger(p))
                        .ToList();
        }

        /// <summary>
        /// Converts the given <see cref="Type"/> instance to the list of underlying enum values of <see cref="long"/>.
        /// </summary>
        /// <param name="type"><see cref="Type"/> instance.</param>
        /// <returns>Returns the list of underlying enum values of <see cref="long"/>.</returns>
        public static List<IOpenApiAny> ToOpenApiInt64Collection(this Type type)
        {
            if (!type.IsUnflaggedEnumType())
            {
                return null;
            }

            var names = Enum.GetValues(type).Cast<long>();

            return names.Select(p => (IOpenApiAny)new OpenApiLong(p))
                        .ToList();
        }

        /// <summary>
        /// Converts the given <see cref="Type"/> instance to the list of underlying enum values of <see cref="byte"/>.
        /// </summary>
        /// <param name="type"><see cref="Type"/> instance.</param>
        /// <returns>Returns the list of underlying enum values of <see cref="byte"/>.</returns>
        public static List<IOpenApiAny> ToOpenApiByteCollection(this Type type)
        {
            if (!type.IsUnflaggedEnumType())
            {
                return null;
            }

            var names = Enum.GetValues(type).Cast<byte>();

            return names.Select(p => (IOpenApiAny)new OpenApiInteger((int)p))
                        .ToList();
        }

        /// <summary>
        /// Converts the given <see cref="Type"/> instance to the list of underlying enum value.
        /// </summary>
        /// <param name="type"><see cref="Type"/> instance.</param>
        /// <returns>Returns the list of underlying enum value.</returns>
        public static List<IOpenApiAny> ToOpenApiIntegerCollection(this Type type)
        {
            if (!type.IsUnflaggedEnumType())
            {
                return null;
            }

            var values = Enum.GetValues(type);
            if (type.GetEnumUnderlyingType() == typeof(short))
            {
                return values.Cast<short>()
                             .Select(p => (IOpenApiAny)new OpenApiInteger(p))
                             .ToList();
            }

            if (type.GetEnumUnderlyingType() == typeof(int))
            {
                return values.Cast<int>()
                             .Select(p => (IOpenApiAny)new OpenApiInteger(p))
                             .ToList();
            }

            if (type.GetEnumUnderlyingType() == typeof(long))
            {
                return values.Cast<long>()
                             .Select(p => (IOpenApiAny)new OpenApiLong(p))
                             .ToList();
            }

            return null;
        }

        /// <summary>
        /// Checks whether the given type is exception or not, from the OpenAPI perspective.
        /// </summary>
        /// <param name="type"><see cref="Type"/> instance.</param>
        /// <returns>Returns <c>True</c>, if the type is identified as exception; otherwise returns <c>False</c>.</returns>
        public static bool IsOpenApiException(this Type type)
        {
            if (type.IsNullOrDefault())
            {
                return false;
            }

            return typeof(Exception).IsAssignableFrom(type);
        }

        /// <summary>
        /// Checks whether the given type is array or not, from the OpenAPI perspective.
        /// </summary>
        /// <param name="type"><see cref="Type"/> instance.</param>
        /// <returns>Returns <c>True</c>, if the type is identified as array; otherwise returns <c>False</c>.</returns>
        public static bool IsOpenApiArray(this Type type)
        {
            if (type.IsNullOrDefault())
            {
                return false;
            }

            return type.IsArrayType();
        }

        /// <summary>
        /// Checks whether the given type is array or not, from the OpenAPI perspective.
        /// </summary>
        /// <param name="type"><see cref="Type"/> instance.</param>
        /// <returns>Returns <c>True</c>, if the type is identified as array; otherwise returns <c>False</c>.</returns>
        public static bool IsOpenApiDictionary(this Type type)
        {
            if (type.IsNullOrDefault())
            {
                return false;
            }

            return type.IsDictionaryType();
        }

        /// <summary>
        /// Checks whether the given type is nullable or not.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>Returns <c>True</c>, if the given type is nullable; otherwise returns <c>False</c>.</returns>
        public static bool IsOpenApiNullable(this Type type)
        {
            return type.IsNullableType(out var underlyingType);
        }

        /// <summary>
        /// Checks whether the given type is nullable or not.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <param name="underlyingType">Underlying type to return</param>
        /// <returns>Returns <c>True</c>, if the given type is nullable; otherwise returns <c>False</c>.</returns>
        public static bool IsOpenApiNullable(this Type type, out Type underlyingType)
        {
            if (type.IsNullOrDefault())
            {
                underlyingType = null;

                return false;
            }

            return type.IsNullableType(out underlyingType);
        }

        /// <summary>
        /// Checks whether the given enum type has <see cref="JsonConverterAttribute"/> or not.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="JsonConverter"/>.</typeparam>
        /// <param name="type">Enum type.</param>
        /// <returns>Returns <c>True</c>, if the given enum type has <see cref="JsonConverterAttribute"/>; otherwise, returns <c>False</c>.</returns>
        public static bool HasJsonConverterAttribute<T>(this Type type) where T : JsonConverter
        {
            var attribute = type.GetCustomAttribute<JsonConverterAttribute>(inherit: false);
            if (attribute.IsNullOrDefault())
            {
                return false;
            }

            return typeof(T).IsAssignableFrom(attribute.ConverterType);
        }

        /// <summary>
        /// Gets the OpenAPI reference ID.
        /// </summary>
        /// <param name="type"><see cref="Type"/> instance.</param>
        /// <param name="isDictionary">Value indicating whether the type is <see cref="Dictionary{TKey, TValue}"/> or not.</param>
        /// <param name="isList">Value indicating whether the type is <see cref="List{T}"/> or not.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance.</param>
        /// <returns>Returns the OpenAPI reference ID.</returns>
        public static string GetOpenApiReferenceId(this Type type, bool isDictionary, bool isList, NamingStrategy namingStrategy = null)
        {
            if (namingStrategy.IsNullOrDefault())
            {
                namingStrategy = new DefaultNamingStrategy();
            }

            if (isDictionary)
            {
                var name = type.Name.EndsWith("[]")
                    ? "Dictionary_" + type.GetOpenApiTypeName(namingStrategy)
                    : type.GetOpenApiTypeName(namingStrategy);
                return namingStrategy.GetPropertyName(name, hasSpecifiedName: false);
            }
            if (isList)
            {
                var name = type.Name.EndsWith("[]")
                    ? "List_" + type.GetOpenApiTypeName(namingStrategy)
                    : type.GetOpenApiTypeName(namingStrategy);
                return namingStrategy.GetPropertyName(name, hasSpecifiedName: false);
            }

            if (type.IsGenericType)
            {
                return type.GetOpenApiTypeName(namingStrategy);
            }

            return namingStrategy.GetPropertyName(type.Name, hasSpecifiedName: false);
        }

        /// <summary>
        /// Gets the OpenAPI root reference ID.
        /// </summary>
        /// <param name="type"><see cref="Type"/> instance.</param>
        /// <returns>Returns the OpenAPI root reference ID.</returns>
        public static string GetOpenApiRootReferenceId(this Type type)
        {
            if (!type.IsGenericType)
            {
                return type.Name;
            }

            return type.GetOpenApiGenericRootName();
        }

        /// <summary>
        /// Checks whether the given type is generic type of
        /// </summary>
        /// <param name="t1"><see cref="Type"/> instance.</param>
        /// <param name="t2"><see cref="Type"/> instance to compare.</param>
        /// <returns>Returns <c>True</c>, if the given type is generic; otherwise returns <c>False</c>.</returns>
        public static bool IsGenericTypeOf(this Type t1, Type t2)
        {
            return t1.IsGenericType && t1.GetGenericTypeDefinition() == t2;
        }

        /// <summary>
        /// Gets the underlying type of the given generic type.
        /// </summary>
        /// <param name="type">Type to check to get its underlying type.</param>
        /// <returns>Returns the underlying type.</returns>
        public static Type GetUnderlyingType(this Type type)
        {
            var underlyingType = default(Type);
            if (type.IsOpenApiNullable(out var nullableUnderlyingType))
            {
                underlyingType = nullableUnderlyingType;
            }

            if (type.IsOpenApiArray())
            {
                underlyingType = type.GetElementType() ?? type.GetGenericArguments()[0];
            }

            if (type.IsOpenApiDictionary())
            {
                underlyingType = type.GetGenericArguments()[1];
            }

            if (underlyingType.IsOpenApiNullable(out var nullableUnderlyingTypeOfUnderlyingType))
            {
                underlyingType = nullableUnderlyingTypeOfUnderlyingType;
            }

            return underlyingType;
        }

        /// <summary>
        /// Gets the OpenAPI description from the given <see cref="Type"/>.
        /// </summary>
        /// <param name="type"><see cref="Type"/> instance.</param>
        /// <returns>Returns the OpenAPI description from the given <see cref="Type"/>.</returns>
        public static string GetOpenApiDescription(this Type type)
        {
            if (type.IsOpenApiDictionary())
            {
                return $"Dictionary of {type.GetOpenApiSubTypeName()}";
            }

            if (type.IsOpenApiArray())
            {
                return $"Array of {type.GetOpenApiSubTypeName()}";
            }

            if (type.IsGenericType)
            {
                return $"{type.GetOpenApiGenericRootName()} containing {type.GetOpenApiSubTypeNames()}";
            }

            return type.Name;
        }

        /// <summary>
        /// Gets the root name of the given generic type.
        /// </summary>
        /// <param name="type"><see cref="Type"/> instance.</param>
        /// <returns>Returns the root name of the given generic type.</returns>
        public static string GetOpenApiGenericRootName(this Type type)
        {
            if (!type.IsGenericType)
            {
                return type.Name;
            }

            var name = type.Name.Split(new[] { "`" }, StringSplitOptions.RemoveEmptyEntries).First();

            return name;
        }

        /// <summary>
        /// Gets the type name applied by <see cref="NamingStrategy"/>.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance.</param>
        /// <returns>Returns the type name applied by <see cref="NamingStrategy"/>.</returns>
        public static string GetOpenApiTypeName(this Type type, NamingStrategy namingStrategy = null)
        {
            if (namingStrategy.IsNullOrDefault())
            {
                namingStrategy = new DefaultNamingStrategy();
            }

            var name = string.Join("_", GetTypeNames(type, namingStrategy));

            return name;
        }

        /// <summary>
        /// Gets the sub type of the given <see cref="Type"/>.
        /// </summary>
        /// <param name="type"><see cref="Type"/> instance.</param>
        /// <returns>Returns the sub type of the given <see cref="Type"/>.</returns>
        public static Type GetOpenApiSubType(this Type type)
        {
            if (type.IsDictionaryType())
            {
                return type.GetGenericArguments()[1];
            }

            if (type.BaseType == typeof(Array))
            {
                return type.GetElementType();
            }

            if (type.IsArrayType())
            {
                return type.GetGenericArguments()[0];
            }

            return null;
        }

        /// <summary>
        /// Gets the name of the sub type of the given <see cref="Type"/>.
        /// </summary>
        /// <param name="type"><see cref="Type"/> instance.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance.</param>
        /// <returns>Returns the name of the sub type of the given <see cref="Type"/>.</returns>
        public static string GetOpenApiSubTypeName(this Type type, NamingStrategy namingStrategy = null)
        {
            if (namingStrategy.IsNullOrDefault())
            {
                namingStrategy = new DefaultNamingStrategy();
            }

            if (type.IsDictionaryType())
            {
                var name = type.GetGenericArguments()[1].Name;

                return namingStrategy.GetPropertyName(name, hasSpecifiedName: false);
            }

            if (type.BaseType == typeof(Array))
            {
                var name = type.GetElementType().Name;

                return namingStrategy.GetPropertyName(name, hasSpecifiedName: false);
            }

            if (type.IsArrayType())
            {
                var name = type.GetGenericArguments()[0].Name;

                return namingStrategy.GetPropertyName(name, hasSpecifiedName: false);
            }

            return null;
        }

        /// <summary>
        /// Gets the list of names of the sub type of the given <see cref="Type"/>.
        /// </summary>
        /// <param name="type"><see cref="Type"/> instance.</param>
        /// <returns>Returns the list of names of the sub type of the given <see cref="Type"/>.</returns>
        public static string GetOpenApiSubTypeNames(this Type type)
        {
            if (!type.IsGenericType)
            {
                return null;
            }

            var types = type.GetGenericArguments().ToList();
            if (types.Count == 1)
            {
                return types[0].GetOpenApiGenericRootName();
            }

            var names = (string)null;
            for (var i = 0; i < types.Count - 1; i++)
            {
                names += $"{types[i].GetOpenApiGenericRootName()}, ";
            }
            names += $"and {types[types.Count - 1].GetOpenApiGenericRootName()}";

            return names;
        }

        /// <summary>
        /// Checks whether the given type has a recursive property or not.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>Returns <c>True</c>, if the given type has a recursive property; otherwise returns <c>False</c>.</returns>
        public static bool HasRecursiveProperty(this Type type)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                 .Where(p => !p.ExistsCustomAttribute<JsonIgnoreAttribute>());

            var hasRecursiveType = properties.Select(p => p.PropertyType)
                                             .Any(p => p == type);

            return hasRecursiveType;
        }

        /// <summary>
        /// Checks whether the given type implements the given interface type.
        /// </summary>
        /// <typeparam name="T">Type of interface.</typeparam>
        /// <param name="type">Type to check.</param>
        /// <returns>Returns <c>True</c>, if the given type implements the given interface type; otherwise returns <c>False</c>.</returns>
        public static bool HasInterface<T>(this Type type)
        {
            if (type.IsNullOrDefault())
            {
                return false;
            }

            var name = typeof(T).Name;

            return HasInterface(type, name);
        }

        /// <summary>
        /// Checks whether the given type implements the given interface type.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <param name="interfaceName">Name of the interface.</param>
        /// <returns>Returns <c>True</c>, if the given type implements the given interface; otherwise returns <c>False</c>.</returns>
        public static bool HasInterface(this Type type, string interfaceName)
        {
            if (type.IsNullOrDefault())
            {
                return false;
            }

            var @interface = type.GetInterface(interfaceName, ignoreCase: true);
            if (@interface.IsNullOrDefault())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks whether the given type has the given attribute type or not.
        /// </summary>
        /// <typeparam name="T">Type of attribute.</typeparam>
        /// <param name="type">Any type.</param>
        /// <returns>Returns <c>True</c>, if the given type has the given attribute type; otherwise returns <c>False</c>.</returns>
        public static bool HasCustomAttribute<T>(this Type type) where T : Attribute
        {
            if (type.IsNullOrDefault())
            {
                return false;
            }

            return HasCustomAttribute(type, typeof(T));
        }

        /// <summary>
        /// Checks whether the given type has the given attribute type or not.
        /// </summary>
        /// <param name="type">Any type.</param>
        /// <param name="attributeType">Any attribute type.</param>
        /// <returns>Returns <c>True</c>, if the given type has the given attribute type; otherwise returns <c>False</c>.</returns>
        public static bool HasCustomAttribute(this Type type, Type attributeType)
        {
            if (type.IsNullOrDefault())
            {
                return false;
            }

            var attribute = type.GetCustomAttribute(attributeType, inherit: false);
            if (attribute.IsNullOrDefault())
            {
                return false;
            }

            return true;
        }

        private static bool IsArrayType(this Type type)
        {
            var isArrayType = type.Name.Equals("String", StringComparison.InvariantCultureIgnoreCase) == false &&
                              type.GetInterfaces()
                                  .Where(p => p.IsInterface)
                                  .Where(p => p.Name.Equals("IEnumerable", StringComparison.InvariantCultureIgnoreCase) == true)
                                  .Any() &&
                              type.IsJObjectType() == false &&
                              type.IsDictionaryType() == false;

            return isArrayType;
        }

        private static HashSet<string> dictionaries = new HashSet<string>
        {
            "Dictionary`2",
            "IDictionary`2",
            "IReadOnlyDictionary`2",
            "KeyValuePair`2",
        };

        private static bool IsDictionaryType(this Type type)
        {
            var isDictionaryType = type.IsGenericType &&
                                   dictionaries.Any(p => type.Name.Equals(p, StringComparison.InvariantCultureIgnoreCase) == true);

            return isDictionaryType;
        }

        private static bool IsNullableType(this Type type, out Type underlyingType)
        {
            underlyingType = Nullable.GetUnderlyingType(type);

            return !underlyingType.IsNullOrDefault();
        }

        private static IEnumerable<string> GetTypeNames(Type type, NamingStrategy namingStrategy)
        {
            if (type.IsGenericType)
            {
                yield return namingStrategy.GetPropertyName(GetOpenApiGenericRootName(type), false);

                foreach (var argType in type.GetGenericArguments())
                {
                    foreach (var name in GetTypeNames(argType, namingStrategy))
                    {
                        yield return namingStrategy.GetPropertyName(name, false);
                    }
                }
            }
            else
            {
                if (type.Name.EndsWith("[]"))
                {
                    yield return namingStrategy.GetPropertyName(type.Name.Substring(0, type.Name.Length - 2), false);
                }
                else
                {
                    yield return namingStrategy.GetPropertyName(type.Name, false);
                }
            }
        }
    }
}
