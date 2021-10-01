using System.Collections.Generic;
using System.Linq;

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
        /// Gets the <see cref="IHeaderDictionary"/> instance from the <see cref="HttpRequestData"/>.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestData"/> instance.</param>
        /// <returns>Returns <see cref="IHeaderDictionary"/> instance.</returns>
        public static IHeaderDictionary Headers(this HttpRequestData req)
        {
            req.ThrowIfNullOrDefault();

            var headers = req.Headers.ToDictionary(p => p.Key, p => new StringValues(p.Value.ToArray()));
            if (headers.IsNullOrDefault() || headers.Any() == false)
            {
                headers = new Dictionary<string, StringValues>();
            }

            return new HeaderDictionary(headers);
        }

        /// <summary>
        /// Gets the <see cref="StringValues"/> object from the header of <see cref="HttpRequestData"/>.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestData"/> instance.</param>
        /// <param name="key">Header key.</param>
        /// <returns>Returns <see cref="StringValues"/> object.</returns>
        public static StringValues Header(this HttpRequestData req, string key)
        {
            req.ThrowIfNullOrDefault();

            var headers = Headers(req);
            var value = headers.ContainsKey(key) ? headers[key] : new StringValues(default(string));

            return value;
        }

        /// <summary>
        /// Gets the <see cref="IQueryCollection"/> instance from the <see cref="HttpRequestData"/>.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestData"/> instance.</param>
        /// <returns>Returns <see cref="IQueryCollection"/> instance.</returns>
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
        /// Gets the <see cref="StringValues"/> object from the querystring of <see cref="HttpRequestData"/>.
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
