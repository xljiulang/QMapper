﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace QMapper
{
    /// <summary>
    /// 提供高效的属性转换器
    /// </summary>
    abstract class Converter
    {
        /// <summary>
        /// 转换器实例
        /// </summary>
        private static readonly Converter converter;

        /// <summary>
        /// 下一个转换器
        /// </summary>
        public Converter Next { get; set; }

        /// <summary>
        /// 执行转换
        /// </summary>
        /// <param name="context">上下文</param> 
        /// <returns></returns>
        public abstract Expression Invoke(Context context);


        /// <summary>
        /// 静态构造器
        /// </summary>
        static Converter()
        {
            var converters = new Converter[]
            {
                new TypeEqualsConverter(),
                new ValueTypeConverter(),
                new StringTargetConverter(),
                new EnumTargetConverter(),
                new ConvertibleConverter(),
                new OthersTargetConverter()
            };

            converters.Aggregate((pre, next) =>
            {
                pre.Next = next;
                return next;
            }).Next = new NotSupportedConverter();

            converter = converters.First();
        }

        /// <summary>
        /// 检测如果值不为null则使用then值
        /// 则否根据目标类型返回null或抛出不支持的异常
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="thenValue"></param>
        /// <returns></returns>
        protected Expression IfValueIsNotNullThen(Context context, Expression thenValue)
        {
            if (context.Source.IsNotNullValueType == true)
            {
                return thenValue;
            }

            var value = Expression.Variable(context.Target.Type, "value");
            var condition = Expression.IfThenElse(
                Expression.Equal(context.Value, Expression.Constant(null, context.Source.Type)),
                Expression.IfThenElse(Expression.IsFalse(
                    Expression.Constant(context.Target.IsNotNullValueType)),
                    Expression.Assign(value, Expression.Constant(null, context.Target.Type)),
                    Expression.Throw(Expression.Constant(new NotSupportedException($"不支持null值的{context.Source.Info}转换为{context.Target.Info}")))
                ),
                Expression.Assign(value, thenValue)
            );

            return Expression.Block(new ParameterExpression[] { value }, condition, value);
        }

        /// <summary>
        /// 调用静态转换方法
        /// object ConvertMethod(object value, Type targetNotNullType)
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="methodName">转换方法名</param>
        /// <returns></returns>
        protected Expression CallStaticConvert(Context context, string methodName)
        {
            var method = this.GetType().GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            var value = Expression.Convert(context.Value, typeof(object));
            var targetType = Expression.Constant(context.Target.NotNullType);

            var result = Expression.Call(null, method, value, targetType);
            return Expression.Convert(result, context.Target.Type);
        }

        /// <summary>
        /// 表达式类型转换
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public static Expression Convert(Context context)
        {
            return converter.Invoke(context);
        }
    }
}
