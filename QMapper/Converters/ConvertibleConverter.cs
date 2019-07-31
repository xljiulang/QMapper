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
            if (context.ValueNotNullType.IsInheritFrom<IConvertible>() == false)
            {
                return this.Next.Invoke(context);
            }

            if (context.TargetNotNullType.IsInheritFrom<IConvertible>() == false)
            {
                return this.Next.Invoke(context);
            }

            // 非空值类型之间相互转换 (int)(long value)
            if (context.ValueIsNotNullValueType && context.TargetIsNotNullValueType)
            {
                return Expression.Convert(context.Value, context.TargetType);
            }

            var method = this.GetStaticMethod($"{nameof(ConverToConvertible)}");
            var valueArg = Expression.Convert(context.Value, typeof(object));
            var targetTypeArg = Expression.Constant(context.TargetNotNullType);

            var value = Expression.Call(null, method, valueArg, targetTypeArg);
            return Expression.Convert(value, context.TargetType);
        }

        /// <summary>
        /// 将IConvertible转换为IConvertible类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetNotNullType"></param>
        /// <returns></returns>
        private static object ConverToConvertible(object value, Type targetNotNullType)
        {
            if (value == null)
            {
                return null;
            }

            var convertible = value as IConvertible;
            return convertible.ToType(targetNotNullType, null);
        }
    }
}
