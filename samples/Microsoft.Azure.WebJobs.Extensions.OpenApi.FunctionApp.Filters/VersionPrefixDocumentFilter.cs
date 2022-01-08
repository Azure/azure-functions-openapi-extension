using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Filters
{
    /// <summary>
    /// This is an example implementation for a document filter to demonstrate the ability of modifying the IDocumentFilter
    /// after it has been created.
    ///
    /// This filter prepends all paths in the OpenApi document when a request with query string like ?tag=v1,name is made.
    /// </summary>
    public class VersionPrefixDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument document, IHttpRequestDataObject request)
        {
            var tags = request.Query["tag"].ToArray(",");

            if (tags.Length > 1 && Regex.IsMatch(tags[0], "^v[0-9]$"))
            {
                foreach (var path in document.Paths.Keys.ToList())
                {
                    var currentPath = document.Paths[path];

                    document.Paths.Remove(path);
                    document.Paths[$"/{tags[0]}{path}"] = currentPath;
                }
            }
        }
    }
}
