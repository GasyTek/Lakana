using System;
using System.Linq.Expressions;
using System.Reflection;

namespace GasyTek.Lakana.Common.Extensions
{
    /// <summary>
    /// Extension methods
    /// </summary>
    public static class PropertyExtensions
    {
        /// <summary>
        /// Extension methods that extracts property name from a linq expression.
        /// </summary>
        public static string GetPropertyName<TObject>(this TObject type, Expression<Func<TObject, object>> propertyRefExpr)
        {
            return GetPropertyNameCore(propertyRefExpr.Body);
        }

        private static string GetPropertyNameCore(Expression propertyRefExpr)
        {
            if (propertyRefExpr == null)
                throw new ArgumentNullException("propertyRefExpr", @"propertyRefExpr is null.");

            var memberExpr = propertyRefExpr as MemberExpression;
            if (memberExpr == null)
            {
                var unaryExpr = propertyRefExpr as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                    memberExpr = unaryExpr.Operand as MemberExpression;
            }

            if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
                return memberExpr.Member.Name;

            throw new ArgumentException(@"No property reference expression was found.",
                                        "propertyRefExpr");
        }


        /// <summary>
        /// Transforms an expression to a property info if possible
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static PropertyInfo ToPropertyInfo<TTarget, TValue>(this Expression<Func<TTarget, TValue>> expression)
        {
            var body = expression.Body;

            if (body.NodeType != ExpressionType.MemberAccess)
            {
                throw new ArgumentException(@"Property expression must be of the form 'x => x.SomeProperty'", "expression");
            }

            // Cast the expression to the appropriate type
            var memberExpression = (MemberExpression)body;
            return memberExpression.Member as PropertyInfo;
        }
    }
}