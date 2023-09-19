using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors
{
    /// <summary>
    /// This represents the type visitor for <see cref="object"/>.
    /// </summary>
    public class BaseObjectTypeVisitor : TypeVisitor
    {
        /// <inheritdoc />
        public BaseObjectTypeVisitor(VisitorCollection visitorCollection)
            : base(visitorCollection)
        {
        }

        /// <inheritdoc />
        public override bool IsVisitable(Type type)
        {
            var isVisitable = this.IsVisitable(type, TypeCode.Object) && type == typeof(object);

            return isVisitable;
        }

        /// <inheritdoc />
        public override void Visit(IAcceptor acceptor, KeyValuePair<string, Type> type, NamingStrategy namingStrategy, bool useFullName = false, params Attribute[] attributes)
        {
            var title = type.Value.IsGenericType
                ? namingStrategy.GetPropertyName(type.Value.GetTypeName(useFullName).Split('`').First(), hasSpecifiedName: false) + "_" +
                  string.Join("_",
                      type.Value.GenericTypeArguments.Select(a => namingStrategy.GetPropertyName(a.GetTypeName(useFullName), false)))
                : namingStrategy.GetPropertyName(type.Value.GetTypeName(useFullName), hasSpecifiedName: false);
            this.Visit(acceptor, name: type.Key, title: title, dataType: "object", dataFormat: null, attributes: attributes);
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
            return this.PayloadVisit(dataType: "object", dataFormat: null);
        }
    }
}
