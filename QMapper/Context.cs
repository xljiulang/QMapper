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
        public TypeInfo Source { get; }

        /// <summary>
        /// 获取目标类型信息
        /// </summary>
        public TypeInfo Target { get; }

        /// <summary>
        /// 转换上下文
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <param name="targetType">目标类型</param>
        public Context(Expression value, Type targetType)
        {
            this.Value = value;
            this.Source = new TypeInfo(value.Type);
            this.Target = new TypeInfo(targetType);
        }

        /// <summary>
        /// 表示类型信息
        /// </summary>
        public class TypeInfo
        {
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
            public bool IsNotNullValueType
            {
                get => this.IsValueType && this.Type == this.NotNullType;
            }


            /// <summary>
            /// 类型信息
            /// </summary>
            /// <param name="type"></param>
            public TypeInfo(Type type)
            {
                this.Type = type;
                this.NotNullType = Nullable.GetUnderlyingType(type) ?? type;
                this.IsValueType = type.GetTypeInfo().IsValueType;
            }
        }
    }
}
