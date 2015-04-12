//--------------------------------------------------------------------------------------
// <copyright file="PropertyReflection.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Utility
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Get the string name and type of a property or field. 
    /// <code>
    /// <![CDATA[
    /// string name = Property.Name<string>(x => x.Length);
    /// ]]>
    /// </code> 
    /// <see cref="http://www.martinwilley.com/net/code/reflection/staticreflection.html"/>
    /// </summary>
    public static class ReflectionHelper
    {
        public static string GetNameFromExpression<T>(Expression<Func<T, object>> expression)
        {
            if (expression == null)
            {
                return null;
            }

            var me = expression.Body as MemberExpression;
            var ue = expression.Body as UnaryExpression;
            string result = null;

            if (me != null)
            {
                result = me.Member.Name;
            }
            else if (ue != null)
            {
                result = (ue.Operand as MemberExpression).Member.Name;
            }

            return result;
        }                

        /// <summary>
        /// Gets the type for the specified entity property or field. 
        /// <code>
        /// <![CDATA[
        /// Type<string>(x => x.Length) == typeof(int)
        /// ]]>
        /// </code> 
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity (interface or class).</typeparam>
        /// <param name="expression">The expression returning the entity property, in the form x =&gt; x.Id</param>
        /// <returns>A type.</returns>
        public static Type GetTypeFromExpression<T>(Expression<Func<T, object>> expression)
        {
            if (expression == null)
            {
                return null;
            }

            var memberExpression = GetMemberFromExpression(expression);

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo != null)
            {
                return propertyInfo.PropertyType;
            }

            //// not a property, maybe a public field
            var fieldInfo = memberExpression.Member as FieldInfo;
            if (fieldInfo != null)
            {
                return fieldInfo.FieldType;
            }

            return typeof(object);
        }

        /// <summary>
        /// Gets the MemberExpression for the expression
        /// </summary>
        /// <typeparam name="TEntity">
        /// The expression name
        /// </typeparam>
        /// <typeparam name="T">
        /// The expression value
        /// </typeparam>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <returns>
        /// Returns the MemberExpression for the expression
        /// </returns>
        public static MemberExpression GetMemberFromExpression<TClass, TProperty>(Expression<Func<TClass, TProperty>> expression)
        {
            if (expression == null)
            {
                return null;
            }

            ///// originally from Fluent NHibernate
            MemberExpression memberExpression = null;
            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                var body = (UnaryExpression)expression.Body;
                memberExpression = body.Operand as MemberExpression;
            }
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
            }

            //// runtime exception if not a member
            if (memberExpression == null)
            {
                throw new ArgumentException(@"Not a property or field", "expression");
            }

            return memberExpression;
        }

        /// <summary>
        /// Method for extracting a property value from an anonymous class instance.
        /// </summary>
        /// <typeparam name="T">Placeholder for class matching a specific call of the method.</typeparam>
        /// <param name="item">The anonymous instance</param>
        /// <param name="key">The name of the property in the instance</param>
        /// <param name="defaultValue">If the value cannot be extracted then this will be returned</param>
        /// <returns>The value as type of T</returns>
        public static TValue GetValueFromObject<TValue>(object item, string key)
        {
            try
            {
                var type = item.GetType();

                var property = type.GetProperty(key);

                if (property != null)
                {
                    var value = property.GetValue(item, null);
                    return (TValue)value;
                }
            }
            catch
            {
                return default(TValue);
            }

            return default(TValue);
        }

        public static TValue GetValueFromObject<TClass, TValue>(object item, Expression<Func<TClass, object>> expression)
        {
            if (expression == null)
            {
                return default(TValue);
            }

            var name = ReflectionHelper.GetNameFromExpression(expression);

            return GetValueFromObject<TValue>(item, name);
        }

        /// <summary>
        /// Method for adding a parameter to an anonymous class instance after its creation
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The object with the parameter added
        /// </returns>
        public static IDictionary<string, object> AddValueToObject(object item, string name, object value)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();
            var properties = TypeDescriptor.GetProperties(item);
            foreach (PropertyDescriptor property in properties)
            {
                result.Add(property.Name, property.GetValue(item));
            }

            result.Add(name, value);

            return result;
        }
    }
}
