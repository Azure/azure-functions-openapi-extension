using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    public class FakeTypeVisitor : TypeVisitor
    {
        public FakeTypeVisitor(VisitorCollection visitorCollection)
            : base(visitorCollection)
        {
        }

        public bool IsTypeReferential(Type type)
        {
            return this.IsReferential(type);
        }
    }
}
