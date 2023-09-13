using System;
using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors
{
    /// <summary>
    /// This represents the type visitor for <see cref="DateTimeOffset"/>.
    /// </summary>
    public class DateTimeOffsetObjectTypeVisitor : TypeVisitor
    {
        /// <inheritdoc />
        public DateTimeOffsetObjectTypeVisitor(VisitorCollection visitorCollection)
            : base(visitorCollection)
        {
        }

        /// <inheritdoc />
        public override bool IsVisitable(Type type)
        {
            var isVisitable = this.IsVisitable(type, TypeCode.Object) && type == typeof(DateTimeOffset);

            return isVisitable;
        }

        /// <inheritdoc />
        public override void Visit(IAcceptor acceptor, KeyValuePair<string, Type> type, NamingStrategy namingStrategy, bool useFullName = false, params Attribute[] attributes)
        {
            this.Visit(acceptor, name: type.Key, title: null, dataType: "string", dataFormat: "date-time", attributes: attributes);
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
            return this.ParameterVisit(dataType: "string", dataFormat: "date-time");
        }

        /// <inheritdoc />
        public override bool IsPayloadVisitable(Type type)
        {
            var isVisitable = this.IsVisitable(type);

            return isVisitable;
        }

        /// <inheritdoc />
        public override OpenApiSchema PayloadVisit(Type type, NamingStrategy namingStrategy, bool useFullName = false)
        {
            return this.PayloadVisit(dataType: "string", dataFormat: "date-time");
        }
    }
}
