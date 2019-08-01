using System;
using System.Linq.Expressions;

namespace QMapper
{
    /// <summary>
    /// 提供Convertible类型之间转换
    /// </summary>
    class ConvertibleConverter : Converter
    {
        /// <summary>
        /// 执行转换
        /// </summary>
        /// <param name="context">上下文</param> 
        /// <returns></returns>
        public override Expression Invoke(Context context)
        {
            if (context.Source.NotNullType.IsInheritFrom<IConvertible>() == false)
            {
                return this.Next.Invoke(context);
            }

            if (context.Target.NotNullType.IsInheritFrom<IConvertible>() == false)
            {
                return this.Next.Invoke(context);
            }

            var method = this.GetStaticMethod(nameof(ConverToConvertible));
            var value = Expression.Convert(context.Value, typeof(object));
            var targetType = Expression.Constant(context.Target.NotNullType);
            var targetIsNotNullValueType = Expression.Constant(context.Target.IsNotNullValueType);

            var result = Expression.Call(null, method, value, targetType, targetIsNotNullValueType);
            return Expression.Convert(result, context.Target.Type);
        }

        /// <summary>
        /// 将IConvertible转换为IConvertible类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetNotNullType"></param>
        /// <param name="targetIsNotNullValueType">目标类型为非空值类型</param>
        /// <exception cref="NotSupportedException"></exception>
        /// <returns></returns>
        private static object ConverToConvertible(object value, Type targetNotNullType, bool targetIsNotNullValueType)
        {
            if (value == null)
            {
                if (targetIsNotNullValueType == false)
                {
                    return null;
                }
                throw new NotSupportedException($"不支持null值转换为{targetNotNullType}");
            }

            var convertible = value as IConvertible;
            return convertible.ToType(targetNotNullType, null);
        }
    }
}
