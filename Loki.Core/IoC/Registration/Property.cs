using System;
using System.Linq.Expressions;
using System.Reflection;
using Loki.Common;

namespace Loki.IoC.Registration
{
    public class Property<TService> : IProperty<TService>
    {
        public PropertyInfo Key { get; internal set; }

        public object Value { get; internal set; }

        public bool Ignore { get; internal set; }

        public string Name { get; internal set; }

        public static PropertyKey<TService> ForKey<TProperty>(Expression<Func<TService, TProperty>> propertyDefinition)
        {
            Property<TService> buffer = new Property<TService>();
            buffer.Key = ExpressionHelper.GetProperty(propertyDefinition);
            return new PropertyKey<TService>(buffer);
        }

        public static PropertyKey<object> ForKey(PropertyInfo propertyDefinition)
        {
            Property<object> buffer = new Property<object>();
            buffer.Key = propertyDefinition;
            return new PropertyKey<object>(buffer);
        }
    }
}