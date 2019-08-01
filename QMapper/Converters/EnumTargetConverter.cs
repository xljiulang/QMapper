using System;
using System.Linq.Expressions;

namespace QMapper
{
    /// <summary>
    /// 目标为枚举类型的转换器
    /// </summary>
    class EnumTargetConverter : Converter
    {
        /// <summary>
        /// 执行转换
        /// </summary>
        /// <param name="context">上下文</param> 
        /// <returns></returns>
        public override Expression Invoke(Context context)
        {
            if (context.Target.NotNullType.IsInheritFrom<Enum>() == false)
            {
                return this.Next.Invoke(context);
            }

            // (XXEnum?)(int value)  (XXEnum)(int? value)  (XXEnum)(double value)
            if (context.Source.IsValueType == true)
            {
                return Expression.Convert(context.Value, context.Target.Type);
            }

            var method = this.GetStaticMethod($"{nameof(ConvertToEnum)}");
            var valueArg = Expression.Convert(context.Value, typeof(object));
            var targetTypeArg = Expression.Constant(context.Target.NotNullType);

            var value = Expression.Call(null, method, valueArg, targetTypeArg);
            return Expression.Convert(value, context.Target.Type);
        }

        /// <summary>
        /// 将value转换为枚举类型
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <param name="targetNotNullType">转换的目标类型</param>
        /// <exception cref="NotSupportedException"></exception>
        /// <returns></returns>
        private static object ConvertToEnum(object value, Type targetNotNullType)
        {
            if (value == null)
            {
                return null;
            }
            return Enum.Parse(targetNotNullType, value.ToString(), true);
        }
    }
}
