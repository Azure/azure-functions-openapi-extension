using System;
using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors
{
    /// <summary>
    /// This represents the type visitor for <see cref="JObject"/> or <see cref="JToken"/>.
    /// </summary>
    public class JObjectTypeVisitor : TypeVisitor
    {
        /// <inheritdoc />
        public JObjectTypeVisitor(VisitorCollection visitorCollection)
            : base(visitorCollection)
        {
        }

        /// <inheritdoc />
        public override bool IsVisitable(Type type)
        {
            var isVisitable = this.IsVisitable(type, TypeCode.Object) && type.IsJObjectType();

            return isVisitable;
        }

        /// <inheritdoc />
        public override void Visit(IAcceptor acceptor, KeyValuePair<string, Type> type, NamingStrategy namingStrategy, OpenApiNamespaceType namespaceType, params Attribute[] attributes)
        {
            var title = namingStrategy.GetPropertyName(type.Value.GetTypeName(namingStrategy, namespaceType), hasSpecifiedName: false);
            this.Visit(acceptor, name: type.Key, title: title, dataType: "object", dataFormat: null, attributes: attributes);
        }

        /// <inheritdoc />
        public override bool IsParameterVisitable(Type type)
        {
            return false;
        }

        /// <inheritdoc />
        public override bool IsPayloadVisitable(Type type)
        {
            var isVisitable = this.IsVisitable(type);

            return isVisitable;
        }

        /// <inheritdoc />
        public override OpenApiSchema PayloadVisit(Type type, NamingStrategy namingStrategy, OpenApiNamespaceType namespaceType)
        {
            return this.PayloadVisit(dataType: "object", dataFormat: null);
        }
    }
}
