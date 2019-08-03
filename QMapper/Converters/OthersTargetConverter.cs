using System;
using System.Linq.Expressions;

namespace QMapper
{
    /// <summary>
    /// 提供一些常见类型的转换
    /// </summary>
    class OthersTargetConverter : Converter
    {
        /// <summary>
        /// 执行转换
        /// </summary>
        /// <param name="context">上下文</param> 
        /// <returns></returns>
        public override Expression Invoke(Context context)
        {
            if (context.Target.NonNullableType == typeof(Guid))
            {
                var thenValue = this.CallConvertMethod(nameof(ConvertToGuid), context);
                return context.IfValueNotNullThen(thenValue);
            }

            if (context.Target.NonNullableType == typeof(DateTimeOffset))
            {
                var thenValue = this.CallConvertMethod(nameof(ConvertToDateTimeOffset), context);
                return context.IfValueNotNullThen(thenValue);
            }

            if (context.Target.NonNullableType == typeof(Uri) || context.Target.NonNullableType == typeof(Version))
            {
                var thenValue = this.CallConvertMethod(nameof(ConvertToType), context);
                return context.IfValueNotNullThen(thenValue);
            }

            return this.Next.Invoke(context);
        }

        /// <summary>
        /// 将value转换为目标类型
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <exception cref="NotImplementedException"></exception>
        /// <returns></returns>
        private static Guid ConvertToGuid(object value)
        {
            return Guid.Parse(value.ToString());
        }

        /// <summary>
        /// 将value转换为目标类型
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <exception cref="NotImplementedException"></exception>
        /// <returns></returns>
        private static DateTimeOffset ConvertToDateTimeOffset(object value)
        {
            return DateTimeOffset.Parse(value.ToString());
        }

        /// <summary>
        /// 将value转换为目标类型
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <param name="targetNonNullableType">转换的目标类型</param>
        /// <exception cref="NotImplementedException"></exception>
        /// <returns></returns>
        private static object ConvertToType(object value, Type targetNonNullableType)
        {
            if (typeof(Uri) == targetNonNullableType)
            {
                return new Uri(value.ToString(), UriKind.RelativeOrAbsolute);
            }

            if (typeof(Version) == targetNonNullableType)
            {
                return new Version(value.ToString());
            }

            throw new NotImplementedException();
        }
    }
}
