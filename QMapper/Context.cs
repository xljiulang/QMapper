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
        /// 检测如果值不为null则使用then值
        /// 则否根据目标类型返回null或抛出不支持的异常
        /// </summary>
        /// <param name="thenValue"></param>
        /// <returns></returns>
        public Expression IfValueNotNullThen(Expression thenValue)
        {
            if (this.Source.CanBeNullValue == false)
            {
                return thenValue;
            }

            var variable = Expression.Variable(this.Target.Type, "value");
            var nullAssign = this.GetNullValueAssign(variable);
            var thenAssign = Expression.Assign(variable, thenValue);

            var condition = Expression.IfThenElse(
                Expression.Equal(this.Value, Expression.Default(this.Source.Type)),
                nullAssign,
                thenAssign
            );

            return Expression.Block(new ParameterExpression[] { variable }, condition, variable);
        }

        /// <summary>
        /// 返回变量null赋值表达式
        /// </summary>
        /// <param name="variable">变量</param>
        /// <returns></returns>
        private Expression GetNullValueAssign(ParameterExpression variable)
        {            
            if (this.Target.CanBeNullValue == true)
            {
                return Expression.Assign(variable, Expression.Default(this.Target.Type));
            }

            var exception = new NotSupportedException($"不支持null值的{this.Source.Info}转换为{this.Target.Info}");
            return Expression.Throw(Expression.Constant(exception));
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
            public Type NonNullableType { get; }

            /// <summary>
            /// 获取是否为值类型
            /// </summary>
            public bool IsValueType { get; }

            /// <summary>
            /// 获取值是可以为null
            /// </summary>
            public bool CanBeNullValue { get; }

            /// <summary>
            /// 属性信息
            /// </summary>
            /// <param name="info">属性信息</param>          
            public Property(PropertyInfo info)
            {
                var type = info.PropertyType;
                var nonNullableType = Nullable.GetUnderlyingType(type) ?? type;
                var isValueType = type.GetTypeInfo().IsValueType;
                var canBeNullValue = isValueType == false || type != nonNullableType;

                this.Info = info;
                this.Type = type;
                this.NonNullableType = nonNullableType;
                this.IsValueType = isValueType;
                this.CanBeNullValue = canBeNullValue;
            }
        }
    }
}
