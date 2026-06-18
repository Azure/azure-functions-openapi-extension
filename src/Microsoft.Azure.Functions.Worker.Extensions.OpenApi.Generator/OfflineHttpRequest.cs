using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Generator;

/// <summary>
/// Minimal <see cref="IHttpRequestDataObject"/> so the builder can run without a live Functions host.
/// <c>AddServer</c> seeds an internal request that <c>Build()</c> dereferences; this stub satisfies that.
/// </summary>
internal sealed class OfflineHttpRequest : IHttpRequestDataObject
{
    public string Scheme => "https";
    public HostString Host => new("localhost");
    public IHeaderDictionary Headers { get; } = new HeaderDictionary();
    public IQueryCollection Query { get; } = new QueryCollection();
    public Stream Body { get; } = Stream.Null;
}
