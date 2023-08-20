using System;
using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors
{
    /// <summary>
    /// This represents the type visitor for <see cref="byte"/>.
    /// </summary>
    public class ByteTypeVisitor : TypeVisitor
    {
        /// <inheritdoc />
        public ByteTypeVisitor(VisitorCollection visitorCollection)
            : base(visitorCollection)
        {
        }

        /// <inheritdoc />
        public override bool IsVisitable(Type type)
        {
            var isVisitable = this.IsVisitable(type, TypeCode.Byte);

            return isVisitable;
        }

        /// <inheritdoc />
        public override void Visit(IAcceptor acceptor, KeyValuePair<string, Type> type, NamingStrategy namingStrategy, params Attribute[] attributes)
        {
            this.Visit(acceptor, name: type.Key, title: null, dataType: "string", dataFormat: "byte", attributes: attributes);
        }

        /// <inheritdoc />
        public override bool IsParameterVisitable(Type type)
        {
            var isVisitable = this.IsVisitable(type);

            return isVisitable;
        }

        /// <inheritdoc />
        public override OpenApiSchema ParameterVisit(Type type, NamingStrategy namingStrategy)
        {
            return this.ParameterVisit(dataType: "string", dataFormat: "byte");
        }

        /// <inheritdoc />
        public override bool IsPayloadVisitable(Type type)
        {
            var isVisitable = this.IsVisitable(type);

            return isVisitable;
        }

        /// <inheritdoc />
        public override OpenApiSchema PayloadVisit(Type type, NamingStrategy namingStrategy)
        {
            return this.PayloadVisit(dataType: "string", dataFormat: "byte");
        }
    }
}