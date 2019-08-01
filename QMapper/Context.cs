using System;
using System.Linq.Expressions;
using System.Reflection;

namespace QMapper
{
    /// <summary>
    /// 表示转换上下文
    /// </summary>
    class Context
    {
        /// <summary>
        /// 获取要转换的值
        /// </summary>
        public Expression Value { get; }

        /// <summary>
        /// 获取源类型信息
        /// </summary>
        public Property Source { get; }

        /// <summary>
        /// 获取目标类型信息
        /// </summary>
        public Property Target { get; }

        /// <summary>
        /// 转换上下文
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <param name="source">源属性</param>
        /// <param name="target">目标属性</param>
        public Context(Expression value, PropertyInfo source, PropertyInfo target)
        {
            this.Value = value;
            this.Source = new Property(source);
            this.Target = new Property(target);
        }

        /// <summary>
        /// 表示属性信息
        /// </summary>
        public class Property
        {
            /// <summary>
            /// 获取属性详细信息
            /// </summary>
            public PropertyInfo Info { get; }

            /// <summary>
            /// 获取类型
            /// </summary>
            public Type Type { get; }

            /// <summary>
            /// 获取类型对应的非空类型
            /// </summary>
            public Type NotNullType { get; }

            /// <summary>
            /// 获取是否为值类型
            /// </summary>
            public bool IsValueType { get; }

            /// <summary>
            /// 获取是否为非空的值类型
            /// </summary>
            public bool IsNotNullValueType { get; }

            /// <summary>
            /// 属性信息
            /// </summary>
            /// <param name="info">属性信息</param>          
            public Property(PropertyInfo info)
            {
                var type = info.PropertyType;
                var notNullType = Nullable.GetUnderlyingType(type) ?? type;
                var isValueType = type.GetTypeInfo().IsValueType;
                var isNotNullValueType = isValueType && type == notNullType;

                this.Info = info;
                this.Type = type;
                this.NotNullType = notNullType;
                this.IsValueType = isValueType;
                this.IsNotNullValueType = isNotNullValueType;
            }
        }
    }
}
