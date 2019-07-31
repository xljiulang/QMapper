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
        /// 获取要转换的类型
        /// </summary>
        public Type ValueType { get; }

        /// <summary>
        /// 获取要转换的类型对应的非可空类型
        /// </summary>
        public Type ValueNotNullType { get; }

        /// <summary>
        /// 获取要转换的类型是否为非空值类型
        /// </summary>
        public bool ValueIsNotNullValueType { get; }

        /// <summary>
        /// 获取目标类型
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        /// 获取目标类型对应的非空可类型
        /// </summary>
        public Type TargetNotNullType { get; }

        /// <summary>
        /// 获取目标类型是否为非空值类型
        /// </summary>
        public bool TargetIsNotNullValueType { get; }

        /// <summary>
        /// 转换上下文
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <param name="targetType">目标类型</param>
        public Context(Expression value, Type targetType)
        {
            this.Value = value;
            this.ValueType = value.Type;
            this.TargetType = targetType;

            var valueUnderlyingType = Nullable.GetUnderlyingType(value.Type);
            this.ValueNotNullType = valueUnderlyingType ?? value.Type;
            this.ValueIsNotNullValueType = valueUnderlyingType == null && value.Type.GetTypeInfo().IsValueType;

            var targetUnderlyingType = Nullable.GetUnderlyingType(targetType);
            this.TargetNotNullType = targetUnderlyingType ?? targetType;
            this.TargetIsNotNullValueType = targetUnderlyingType == null && targetType.GetTypeInfo().IsValueType;
        }
    }
}
