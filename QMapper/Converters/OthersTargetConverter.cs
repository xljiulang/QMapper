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
            if (context.Target.NotNullType == typeof(Guid))
            {
                return this.CallStaticConvertIfNotNull(context, nameof(ConvertToGuid));
            }

            if (context.Target.NotNullType == typeof(DateTimeOffset))
            {
                return this.CallStaticConvertIfNotNull(context, nameof(ConvertToDateTimeOffset));
            }

            if (context.Target.NotNullType == typeof(Uri) || context.Target.NotNullType == typeof(Version))
            {
                return this.CallStaticConvertIfNotNull(context, nameof(ConvertToType));
            }

            return this.Next.Invoke(context);
        }

        /// <summary>
        /// 将value转换为目标类型
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <param name="targetNotNullType">转换的目标类型</param>
        /// <exception cref="NotImplementedException"></exception>
        /// <returns></returns>
        private static Guid ConvertToGuid(object value, Type targetNotNullType)
        {
            return Guid.Parse(value.ToString());
        }

        /// <summary>
        /// 将value转换为目标类型
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <param name="targetNotNullType">转换的目标类型</param>
        /// <exception cref="NotImplementedException"></exception>
        /// <returns></returns>
        private static DateTimeOffset ConvertToDateTimeOffset(object value, Type targetNotNullType)
        {
            return DateTimeOffset.Parse(value.ToString());
        }

        /// <summary>
        /// 将value转换为目标类型
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <param name="targetNotNullType">转换的目标类型</param>
        /// <exception cref="NotImplementedException"></exception>
        /// <returns></returns>
        private static object ConvertToType(object value, Type targetNotNullType)
        {
            if (typeof(Uri) == targetNotNullType)
            {
                return new Uri(value.ToString(), UriKind.RelativeOrAbsolute);
            }

            if (typeof(Version) == targetNotNullType)
            {
                return new Version(value.ToString());
            }

            throw new NotImplementedException();
        }
    }
}
