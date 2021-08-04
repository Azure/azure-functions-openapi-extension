using System;
using System.IO;
using System.Net;

using Microsoft.Azure.Functions.Worker.Http;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Tests.Fakes
{
    public class FakeHttpResponseData : HttpResponseData
    {
        public FakeHttpResponseData(FunctionContext functionContext) : base(functionContext)
        {
        }

        public override HttpStatusCode StatusCode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override HttpHeadersCollection Headers { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override Stream Body { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override HttpCookies Cookies => throw new NotImplementedException();
    }
}
