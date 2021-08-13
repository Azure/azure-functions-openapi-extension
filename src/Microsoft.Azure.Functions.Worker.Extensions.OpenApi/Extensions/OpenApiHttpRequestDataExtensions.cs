using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="HttpRequestData"/>.
    /// </summary>
    public static class OpenApiHttpRequestDataExtensions
    {
        /// <summary>
        /// Gets the <see cref="QueryCollection"/> instance from the <see cref="HttpRequestData"/>.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestData"/> instance.</param>
        /// <returns>Returns <see cref="QueryCollection"/> instance.</returns>
        public static IQueryCollection Queries(this HttpRequestData req)
        {
            req.ThrowIfNullOrDefault();

            var queries = QueryHelpers.ParseNullableQuery(req.Url.Query);
            if (queries.IsNullOrDefault())
            {
                queries = new Dictionary<string, StringValues>();
            }

            return new QueryCollection(queries);
        }

        /// <summary>
        /// Gets the <see cref="StringValues"/> object from the <see cref="HttpRequestData"/>.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestData"/> instance.</param>
        /// <param name="key">Querystring key.</param>
        /// <returns>Returns <see cref="StringValues"/> object.</returns>
        public static StringValues Query(this HttpRequestData req, string key)
        {
            req.ThrowIfNullOrDefault();

            var queries = Queries(req);
            var value = queries.ContainsKey(key) ? queries[key] : new StringValues(default(string));

            return value;
        }
    }
}
