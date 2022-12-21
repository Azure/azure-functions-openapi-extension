using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Tests.Fakes
{
    public class FakeHttpRequestData : HttpRequestData
    {
        public FakeHttpRequestData(FunctionContext functionContext, Uri uri, Dictionary<string, string> headers, List<ClaimsIdentity> identities = null, Stream body = null) : base(functionContext)
        {
            this.Url = uri;
            this.Headers = new HttpHeadersCollection(headers ?? new Dictionary<string, string>());
            this.Identities = identities ?? new List<ClaimsIdentity>();
            this.Body = body;
        }

        public override Stream Body { get; }

        public override HttpHeadersCollection Headers { get; }

        public override IReadOnlyCollection<IHttpCookie> Cookies => throw new NotImplementedException();

        public override Uri Url { get; }

        public override IEnumerable<ClaimsIdentity> Identities { get; }

        public override string Method => throw new NotImplementedException();

        public override HttpResponseData CreateResponse()
        {
            throw new NotImplementedException();
        }
    }
}
