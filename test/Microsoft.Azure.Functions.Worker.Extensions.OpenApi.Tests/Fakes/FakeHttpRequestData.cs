using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;

using Microsoft.Azure.Functions.Worker.Http;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Tests.Fakes
{
    public class FakeHttpRequestData : HttpRequestData
    {
        public FakeHttpRequestData(FunctionContext functionContext, Uri uri, Stream body) : base(functionContext)
        {
            this.Url = uri;
            this.Body = body;
        }

        public override Stream Body { get; }

        public override HttpHeadersCollection Headers => throw new NotImplementedException();

        public override IReadOnlyCollection<IHttpCookie> Cookies => throw new NotImplementedException();

        public override Uri Url { get;}

        public override IEnumerable<ClaimsIdentity> Identities => throw new NotImplementedException();

        public override string Method => throw new NotImplementedException();

        public override HttpResponseData CreateResponse()
        {
            throw new NotImplementedException();
        }
    }
}
