using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors
{
    /// <summary>
    /// This represents the type visitor for <see cref="Type"/>.
    /// </summary>
    public class TypeTypeVisitor : TypeVisitor
    {
        /// <inheritdoc />
        public TypeTypeVisitor(VisitorCollection visitorCollection) : base(visitorCollection)
        {
        }

        /// <inheritdoc />
        public override bool IsVisitable(Type type)
        {
            return type == typeof(Type);
        }

        /// <inheritdoc />
        public override bool IsParameterVisitable(Type type)
        {
            return false;
        }

        /// <inheritdoc />
        public override bool IsNavigatable(Type type)
        {
            return false;
        }

        /// <inheritdoc />
        public override bool IsPayloadVisitable(Type type)
        {
            return false;
        }

        /// <inheritdoc />
        public override void Visit(IAcceptor acceptor, KeyValuePair<string, Type> type, NamingStrategy namingStrategy, params Attribute[] attributes)
        {
            this.Visit(acceptor, name: type.Key, title: null, dataType: "string", dataFormat: null, attributes: attributes);
        }
    }
}
