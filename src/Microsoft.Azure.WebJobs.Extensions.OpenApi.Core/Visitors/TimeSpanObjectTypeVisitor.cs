using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors
{
    public class TimeSpanObjectTypeVisitor : TypeVisitor
    {
        public TimeSpanObjectTypeVisitor(VisitorCollection visitorCollection) : base(visitorCollection)
        {
        }

        /// <inheritdoc />
        public override OpenApiSchema ParameterVisit(Type type, NamingStrategy namingStrategy)
        {
            return this.ParameterVisit(dataType: "string", dataFormat: "timespan");
        }

        /// <inheritdoc />
        public override bool IsVisitable(Type type)
        {
            var isVisitable = this.IsVisitable(type, TypeCode.Object) && type == typeof(TimeSpan);

            return isVisitable;
        }

        /// <inheritdoc />
        public override void Visit(IAcceptor acceptor, KeyValuePair<string, Type> type, NamingStrategy namingStrategy, params Attribute[] attributes)
        {
            this.Visit(acceptor, name: type.Key, title: null, dataType: "string", dataFormat: "timespan", attributes: attributes);
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
            return this.PayloadVisit(dataType: "string", dataFormat: "timespan");
        }
    }
}
