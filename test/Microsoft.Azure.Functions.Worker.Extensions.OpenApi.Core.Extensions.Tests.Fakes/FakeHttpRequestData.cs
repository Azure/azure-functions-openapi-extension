using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;

using Microsoft.Azure.Functions.Worker.Http;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Core.Extensions.Tests.Fakes
{
    public class FakeHttpRequestData : HttpRequestData
    {
        private Uri _uri;

        public FakeHttpRequestData(FunctionContext functionContext)
            : base(functionContext)
        {
        }

        public override Stream Body { get; }

        public override HttpHeadersCollection Headers { get; }

        public override IReadOnlyCollection<IHttpCookie> Cookies { get; }

        public override Uri Url { get { return this._uri; } }

        public override IEnumerable<ClaimsIdentity> Identities { get; }

        public override string Method { get; }

        public override HttpResponseData CreateResponse()
        {
            return new FakeHttpResponseData(this.FunctionContext);
        }

        public void SetUri(Uri uri)
        {
            this._uri = uri;
        }
    }
}
