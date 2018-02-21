using System;
using System.Linq.Expressions;
using System.Reflection;

namespace kalexi.Expressions.Utilities
{
    public static class ExpressionUtilities
    {
        public static MemberExpression GetMemberExpression(this Expression bodyExpression)
        {
            if (bodyExpression == null)
            {
                throw new ArgumentNullException(nameof(bodyExpression));
            }
            return bodyExpression is UnaryExpression unaryExpression
                ? (MemberExpression) unaryExpression.Operand
                : (MemberExpression) bodyExpression;
        }

        public static Func<T, TR> CreateGetter<T, TR>(this Expression<Func<T, TR>> accessExpression)
        {
            if (accessExpression == null)
            {
                throw new ArgumentNullException(nameof(accessExpression));
            }
            var property = accessExpression.GetProperty();
            var parameter = Expression.Parameter(typeof(T));
            var memberAccess = Expression.MakeMemberAccess(parameter, property);
            var lambda = Expression.Lambda<Func<T, TR>>(memberAccess, parameter);
            return lambda.Compile();
        }

        public static Action<T, TR> CreateSetter<T, TR>(this Expression<Func<T, TR>> accessExpression)
        {
            if (accessExpression == null)
            {
                throw new ArgumentNullException(nameof(accessExpression));
            }
            var property = accessExpression.GetProperty();
            var itemParameter = Expression.Parameter(typeof(T));
            var valueParameter = Expression.Parameter(typeof(TR));
            var memberAccess = Expression.MakeMemberAccess(itemParameter, property);
            var assignment = Expression.Assign(memberAccess, valueParameter);
            var lambda = Expression.Lambda<Action<T, TR>>(assignment, itemParameter, valueParameter);
            return lambda.Compile();
        }

        public static PropertyInfo GetProperty<T, TR>(this Expression<Func<T, TR>> accessExpression)
        {
            if (accessExpression == null)
            {
                throw new ArgumentNullException(nameof(accessExpression));
            }
            var memberExpression = accessExpression.Body.GetMemberExpression();
            return (PropertyInfo) memberExpression.Member;
        }

        public static PropertyInfo GetProperty<T>(this Expression<Func<T, object>> accessExpression)
        {
            if (accessExpression == null)
            {
                throw new ArgumentNullException(nameof(accessExpression));
            }
            var memberExpression = accessExpression.Body.GetMemberExpression();
            return (PropertyInfo) memberExpression.Member;
        }

        public static FieldInfo GetField<T, TR>(this Expression<Func<T, TR>> accessExpression)
        {
            if (accessExpression == null)
            {
                throw new ArgumentNullException(nameof(accessExpression));
            }
            var memberExpression = accessExpression.Body.GetMemberExpression();
            return (FieldInfo) memberExpression.Member;
        }

        public static FieldInfo GetField<T>(this Expression<Func<T, object>> accessExpression)
        {
            if (accessExpression == null)
            {
                throw new ArgumentNullException(nameof(accessExpression));
            }
            var memberExpression = accessExpression.Body.GetMemberExpression();
            return (FieldInfo) memberExpression.Member;
        }

        public static MethodInfo GetMethodInfo<T>(this Expression<Func<T, object>> accessExpression)
        {
            if (accessExpression == null)
            {
                throw new ArgumentNullException(nameof(accessExpression));
            }
            var e = accessExpression.Body is UnaryExpression unaryExpression
                ? (MethodCallExpression) unaryExpression.Operand
                : (MethodCallExpression) accessExpression.Body;
            return e.Method;
        }

        public static MethodInfo GetMethodInfo<T>(this Expression<Action<T>> accessExpression)
        {
            if (accessExpression == null)
            {
                throw new ArgumentNullException(nameof(accessExpression));
            }
            var e = (MethodCallExpression) accessExpression.Body;
            return e.Method;
        }
    }
}
