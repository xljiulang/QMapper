using System;
using System.Linq.Expressions;
using System.Reflection;

namespace QMapper
{
    /// <summary>
    /// 目标为string的转换器
    /// </summary>
    class StringTargetConverter : Converter
    {
        /// <summary>
        /// 执行转换
        /// </summary>
        /// <param name="context">上下文</param> 
        /// <returns></returns>
        public override Expression Invoke(Context context)
        {
            if (context.Target.Type != typeof(string))
            {
                return this.Next.Invoke(context);
            }

            // 值类型直接ToString()
            if (context.Source.IsValueType == true)
            {
                var toStringMethod = context.Source.Type.GetMethod($"{nameof(ToString)}", new Type[0]);
                return Expression.Call(context.Value, toStringMethod);
            }

            var method = this.GetStaticMethod($"{nameof(ConvertToString)}");
            var valueArg = Expression.Convert(context.Value, typeof(object));
            return Expression.Call(null, method, valueArg);
        }

        /// <summary>
        /// 将value转换为string类型
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <exception cref="NotSupportedException"></exception>
        /// <returns></returns>
        private static string ConvertToString(object value)
        {
            return value?.ToString();
        }
    }
}
