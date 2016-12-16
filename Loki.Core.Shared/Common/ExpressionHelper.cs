using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

using Loki.Common.Resources;

namespace Loki.Common
{
    public static class ExpressionHelper
    {
        /// <summary>
        /// Gets the property represented by the lambda expression.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="property">An expression that accesses a property.</param>
        /// <returns>The property info.</returns>
        public static PropertyInfo GetProperty<TTarget, TProperty>(Expression<Func<TTarget, TProperty>> property)
        {
            return GetMember(property) as PropertyInfo;
        }

        /// <summary>
        /// Gets the member represented by the lambda expression.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="member">An expression that accesses a member.</param>
        /// <returns>The member info.</returns>
        public static MemberInfo GetMember<TTarget, TProperty>(Expression<Func<TTarget, TProperty>> member)
        {
            MemberExpression memberExpr = null;

            // The Func<TTarget, object> we use returns an object, so first statement can be either
            // a cast (if the field/property does not return an object) or the direct member access.
            if (member.Body.NodeType == ExpressionType.Convert)
            {
                // The cast is an unary expression, where the operand is the
                // actual member access expression.
                memberExpr = ((UnaryExpression)member.Body).Operand as MemberExpression;
            }
            else if (member.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpr = member.Body as MemberExpression;
            }

            return memberExpr.Member;
        }

        public static Expression<Func<TTarget>> New<TTarget>()
        {
            Type type = typeof(TTarget);

            ConstructorInfo builder = type.GetConstructor(Type.EmptyTypes);
            if (builder == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Errors.Utils_ExpressionHelper_NoConstructor, type.FullName));
            }

            return Expression.Lambda<Func<TTarget>>(Expression.New(builder));
        }

        public static bool HasDefaultConstructor(Type type)
        {
            return type.GetConstructor(Type.EmptyTypes) != null;
        }
    }
}