using System;
using System.Linq.Expressions;
using System.Reflection;

namespace kalexi.Expressions.Utilities
{
    public static class ExpressionUtilities
    {
        #region Getters

        public static Func<object, object> CreateUntypedGetter(this PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            var parameter = Expression.Parameter(typeof(object));
            var typedParameter = Expression.Convert(parameter, property.DeclaringType);
            var memberAccess = Expression.MakeMemberAccess(typedParameter, property);
            var expression = property.PropertyType.IsValueType
                ? Expression.Convert(memberAccess, typeof(object))
                : (Expression) memberAccess;
            var lambda = Expression.Lambda<Func<object, object>>(expression, parameter);
            return lambda.Compile();
        }

        public static Func<object, object> CreateUntypedGetter(this FieldInfo field)
        {
            if (field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }
            var parameter = Expression.Parameter(typeof(object));
            var typedParameter = Expression.Convert(parameter, field.DeclaringType);
            var memberAccess = Expression.MakeMemberAccess(typedParameter, field);
            var expression = field.FieldType.IsValueType
                ? Expression.Convert(memberAccess, typeof(object))
                : (Expression) memberAccess;
            var lambda = Expression.Lambda<Func<object, object>>(expression, parameter);
            return lambda.Compile();
        }

        public static Func<T, object> CreateGetter<T>(this PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            var parameter = Expression.Parameter(typeof(T));
            var memberAccess = Expression.MakeMemberAccess(parameter, property);
            var expression = property.PropertyType.IsValueType
                ? Expression.Convert(memberAccess, typeof(object))
                : (Expression) memberAccess;
            var lambda = Expression.Lambda<Func<T, object>>(expression, parameter);
            return lambda.Compile();
        }

        public static Func<T, object> CreateGetter<T>(this FieldInfo field)
        {
            if (field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }
            var parameter = Expression.Parameter(typeof(T));
            var memberAccess = Expression.MakeMemberAccess(parameter, field);
            var expression = field.FieldType.IsValueType
                ? Expression.Convert(memberAccess, typeof(object))
                : (Expression) memberAccess;
            var lambda = Expression.Lambda<Func<T, object>>(expression, parameter);
            return lambda.Compile();
        }

        public static Func<T, TR> CreateGetter<T, TR>(this Expression<Func<T, TR>> accessExpression)
        {
            if (accessExpression == null)
            {
                throw new ArgumentNullException(nameof(accessExpression));
            }
            var property = accessExpression.GetMemberInfo();
            var parameter = Expression.Parameter(typeof(T));
            var memberAccess = Expression.MakeMemberAccess(parameter, property);
            var lambda = Expression.Lambda<Func<T, TR>>(memberAccess, parameter);
            return lambda.Compile();
        }

        #endregion

        #region Setters

        public static Action<T, TR> CreateSetter<T, TR>(this Expression<Func<T, TR>> accessExpression)
        {
            if (accessExpression == null)
            {
                throw new ArgumentNullException(nameof(accessExpression));
            }
            var property = accessExpression.GetMemberInfo();
            var itemParameter = Expression.Parameter(typeof(T));
            var valueParameter = Expression.Parameter(typeof(TR));
            var memberAccess = Expression.MakeMemberAccess(itemParameter, property);
            var assignment = Expression.Assign(memberAccess, valueParameter);
            var lambda = Expression.Lambda<Action<T, TR>>(assignment, itemParameter, valueParameter);
            return lambda.Compile();
        }

        public static Action<object, object> CreateUntypedSetter(this PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            var itemParameter = Expression.Parameter(typeof(object));
            var valueParameter = Expression.Parameter(typeof(object));
            var memberAccess = Expression.MakeMemberAccess(Expression.Convert(itemParameter, property.DeclaringType),
                property);
            var assignment = Expression.Assign(memberAccess, Expression.Convert(valueParameter, property.PropertyType));
            var lambda = Expression.Lambda<Action<object, object>>(assignment, itemParameter, valueParameter);
            return lambda.Compile();
        }

        public static Action<object, object> CreateUntypedSetter(this FieldInfo field)
        {
            if (field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }
            var itemParameter = Expression.Parameter(typeof(object));
            var valueParameter = Expression.Parameter(typeof(object));
            var memberAccess = Expression.MakeMemberAccess(Expression.Convert(itemParameter, field.DeclaringType), field);
            var assignment = Expression.Assign(memberAccess, Expression.Convert(valueParameter, field.FieldType));
            var lambda = Expression.Lambda<Action<object, object>>(assignment, itemParameter, valueParameter);
            return lambda.Compile();
        }

        public static Action<T, object> CreateSetter<T>(this PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            var itemParameter = Expression.Parameter(typeof(T));
            var valueParameter = Expression.Parameter(typeof(object));
            var memberAccess = Expression.MakeMemberAccess(itemParameter, property);
            var assignment = Expression.Assign(memberAccess, Expression.Convert(valueParameter, property.PropertyType));
            var lambda = Expression.Lambda<Action<T, object>>(assignment, itemParameter, valueParameter);
            return lambda.Compile();
        }

        public static Action<T, object> CreateSetter<T>(this FieldInfo field)
        {
            if (field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }
            var itemParameter = Expression.Parameter(typeof(T));
            var valueParameter = Expression.Parameter(typeof(object));
            var memberAccess = Expression.MakeMemberAccess(itemParameter, field);
            var assignment = Expression.Assign(memberAccess, Expression.Convert(valueParameter, field.FieldType));
            var lambda = Expression.Lambda<Action<T, object>>(assignment, itemParameter, valueParameter);
            return lambda.Compile();
        }

        #endregion

        public static MemberInfo GetMemberInfo<T, TR>(this Expression<Func<T, TR>> accessExpression)
        {
            if (accessExpression == null)
            {
                throw new ArgumentNullException(nameof(accessExpression));
            }
            var memberExpression = accessExpression.Body.GetMemberExpression();
            return memberExpression.Member;
        }

        public static MemberInfo GetMemberInfo<T>(this Expression<Func<T, object>> accessExpression)
        {
            if (accessExpression == null)
            {
                throw new ArgumentNullException(nameof(accessExpression));
            }
            var memberExpression = accessExpression.Body.GetMemberExpression();
            return memberExpression.Member;
        }

        public static PropertyInfo GetProperty<T, TR>(this Expression<Func<T, TR>> accessExpression)
            => (PropertyInfo) GetMemberInfo(accessExpression);

        public static PropertyInfo GetProperty<T>(this Expression<Func<T, object>> accessExpression)
            => (PropertyInfo) GetMemberInfo(accessExpression);

        public static FieldInfo GetField<T, TR>(this Expression<Func<T, TR>> accessExpression)
            => (FieldInfo) GetMemberInfo(accessExpression);

        public static FieldInfo GetField<T>(this Expression<Func<T, object>> accessExpression)
            => (FieldInfo) GetMemberInfo(accessExpression);

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

        #region Utilities

        private static MemberExpression GetMemberExpression(this Expression bodyExpression)
        {
            if (bodyExpression == null)
            {
                throw new ArgumentNullException(nameof(bodyExpression));
            }
            return bodyExpression is UnaryExpression unaryExpression
                ? (MemberExpression) unaryExpression.Operand
                : (MemberExpression) bodyExpression;
        }

        #endregion
    }
}
