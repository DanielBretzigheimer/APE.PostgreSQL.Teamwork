// <copyright file="EqualsAndHashCode.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Linq.Expressions;
using System.Reflection;

namespace APE.PostgreSQL.Teamwork.Model
{
    /// <summary>
    /// Generic implementation of equals and GetHashCode.
    ///
    /// <remarks>HashCode is implemented like: A * 31 + B * 31 + C ...</remarks>
    /// Usage:
    /// <code>
    ///     private static readonly EqualsAndHashCode&lt;LotIdentification&gt; EqualsAndHashCode = new EqualsAndHashCode&lt;LotIdentification&gt;(
    ///         m => m.Name,
    ///       m => m.TkGeneration,
    ///       m => m.PMode);
    ///
    ///         public override bool Equals(object obj)
    ///     {
    ///         return EqualsAndHashCode.AreEqual(this, obj);
    ///     }
    ///
    ///     public override int GetHashCode()
    ///     {
    ///         return EqualsAndHashCode.GetHashCode(this);
    ///     }
    ///  </code>
    /// </summary>
    public class EqualsAndHashCode<T>
        where T : class
    {
        private readonly Func<T, T, bool> equals;
        private readonly Func<T, int> getHashCode;
        private readonly Expression<Func<T, object>>[] properties;

        public EqualsAndHashCode(params Expression<Func<T, object>>[] properties)
        {
            // Expression<Func<TProperty>>
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties), "properties == null");
            }

            if (properties.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(properties), "properties.Length == 0");
            }

            this.properties = properties;

            this.equals = this.BuildEquals(properties);
            this.getHashCode = this.BuildGetHashCode(properties);
        }

        public bool AreEqual(T? source, object? target)
        {
            if (source == null || target == null)
            {
                return false;
            }

            if (target as T == null)
            {
                return false;
            }

            return this.equals(source, (T)target);
        }

        public int GetHashCode(T? source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), "source == null");
            }

            return this.getHashCode(source);
        }

        private Func<T, int> BuildGetHashCode(Expression<Func<T, object>>[] properties)
        {
            var getHashCodeMethodInfo = typeof(object).GetMethod("GetHashCode");
            if (getHashCodeMethodInfo == null) throw new InvalidOperationException("GetHashCode of object is null");

            var sourceParameter = Expression.Parameter(typeof(T));
            Expression? currentHashCode = null;

            foreach (var property in properties)
            {
                var propertyName = property.Body as UnaryExpression != null
                    ? ((MemberExpression)((UnaryExpression)property.Body).Operand).Member.Name // get name of value types
                    : ((MemberExpression)property.Body).Member.Name; // get name of reference types
                var getterExpression = Expression.PropertyOrField(sourceParameter, propertyName);

                if (currentHashCode == null)
                {
                    currentHashCode = Expression.Call(getterExpression, getHashCodeMethodInfo);
                }
                else
                {
                    currentHashCode = Expression.Add(
                        Expression.Multiply(Expression.Constant(31), currentHashCode),
                        Expression.Call(getterExpression, getHashCodeMethodInfo));
                }
            }

            return Expression.Lambda<Func<T, int>>(currentHashCode, sourceParameter).Compile();
        }

        private Func<T, T, bool> BuildEquals(Expression<Func<T, object>>[] properties)
        {
            var equalsMethodInfo = typeof(object).GetMethod("Equals", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

            var leftParameter = Expression.Parameter(typeof(T));
            var rightParameter = Expression.Parameter(typeof(T));
            Expression? equalsExpression = null;

            foreach (var property in properties)
            {
                var member = property.Body as UnaryExpression != null
                    ? ((MemberExpression)((UnaryExpression)property.Body).Operand).Member // get name of value types
                    : ((MemberExpression)property.Body).Member; // get name of reference types
                var memberType = member.MemberType == MemberTypes.Property
                    ? ((PropertyInfo)member).PropertyType
                    : ((FieldInfo)member).FieldType;

                var leftGetterExpression = Expression.PropertyOrField(leftParameter, member.Name);
                var rightGetterExpression = Expression.PropertyOrField(rightParameter, member.Name);

                if (equalsExpression == null)
                {
                    if (memberType.IsValueType)
                    {
                        equalsExpression = Expression.Equal(leftGetterExpression, rightGetterExpression); // test equality with ==
                    }
                    else
                    {
                        equalsExpression = Expression.Call(equalsMethodInfo, leftGetterExpression, rightGetterExpression); // test equality with object.Equals(..)
                    }
                }
                else
                {
                    if (memberType.IsValueType)
                    {
                        equalsExpression = Expression.AndAlso(
                            equalsExpression,
                            Expression.Equal(leftGetterExpression, rightGetterExpression)); // test equality with ==
                    }
                    else
                    {
                        equalsExpression = Expression.AndAlso(
                            equalsExpression,
                            Expression.Call(equalsMethodInfo, leftGetterExpression, rightGetterExpression)); // test equality with object.Equals(..)
                    }
                }
            }

            return Expression.Lambda<Func<T, T, bool>>(equalsExpression, leftParameter, rightParameter).Compile();
        }
    }
}