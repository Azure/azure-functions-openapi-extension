using System;
using System.Collections.Generic;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Core.Extensions.Tests.Fakes
{
    public class FakeFunctionContext : FunctionContext
    {
        public override string InvocationId { get; }

        public override string FunctionId { get; }

        public override TraceContext TraceContext { get; }

        public override BindingContext BindingContext { get; }

        public override IServiceProvider InstanceServices { get; set; }

        public override FunctionDefinition FunctionDefinition => throw new NotImplementedException();

        public override IDictionary<object, object> Items { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override IInvocationFeatures Features => throw new NotImplementedException();
    }
}
